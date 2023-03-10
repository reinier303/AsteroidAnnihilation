
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using AK.Wwise;

namespace AsteroidAnnihilation
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Variables")]
        public float currentSpeed;
        public float BaseSpeed;
        private float Acceleration;
        private float Deceleration;
        public float AccelerationMultiplier;
        public float DecelerationMultiplier;
        public float ForceModifier = 100;
        public float MaxVelocityCoeficient;
        public float MaxVelocity;
        public float RotationOffsetMultiplier = 0.35f;

        [Header("Movement Skill Variables")]
        public float DashVelocityMultiplier = 3;
        public float DashDuration = 0.4f;
        public float DashCooldown = 1f;

        public float BoostSpeed = 1.5f;
        public float Fuel;
        public float BoostRegen;
        private bool canDash;
        private bool dashing;


        private float cameraDistance;
        private float screenRatio;

        private Vector3 target;
        private Camera cam;
        [SerializeField] CinemachineConfiner confiner;

        private GameManager gameManager;
        private Player player;
        private PlayerStats playerStats;
        private InputManager inputManager;
        private EquipmentManager equipmentManager;
        private ObjectPooler objectPooler;

        public List<ParticleSystem> Engines;

        public PolygonCollider2D BackgroundCollider;
        private Rigidbody2D rb;
        private Vector2 backGroundSize;

        [SerializeField] private GameObject circle;

        public float offsetMultiplier = 1f;

        public Vector2 MovementInput;

        [SerializeField]private List<Vector3> lastInputs;

        //Audio Hookup
        private float currAngle;


        private void Awake()
        {
            backGroundSize = BackgroundCollider.transform.localScale * 14.5f;
            lastInputs = new List<Vector3>();
            rb = GetComponent<Rigidbody2D>();
            canDash = true;
            //TODO:: IMPORTANT CHECK IF THIS CAN BE ONLY DONE FOR PLAYER OBJECT
            Physics2D.simulationMode = SimulationMode2D.Update;
        }

        private void Start()
        {
            cam = Camera.main;
            SceneManager.sceneLoaded += OnSceneLoaded;
            gameManager = GameManager.Instance;
            equipmentManager = EquipmentManager.Instance;
            objectPooler = ObjectPooler.Instance;
            player = gameManager.RPlayer;
            playerStats = GetComponent<PlayerStats>();
            inputManager = gameManager.RInputManager;
            GetMovementVariables();
            GetBoostVariables();
            //GetCameraDistance();
            Engines = new List<ParticleSystem>();
            foreach (ParticleSystem ps in player.Equipment.GetComponentsInChildren<ParticleSystem>())
            {
                Engines.Add(ps);
            }
            AudioManager.Instance.PlayAudio("Play_PlayerShipEngine", gameObject);
            screenRatio = (float)Screen.height / (float)Screen.width;
        }

        public void ResetMovementVariables()
        {
            dashing = false;
            canDash = true;
            GetMovementVariables();
            GetBoostVariables();
        }

        // called second
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            cam = Camera.main;
        }

        private void GetCameraDistance()
        {
            CinemachineComponentBase componentBase = confiner.VirtualCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent(CinemachineCore.Stage.Body);
            if (componentBase is CinemachineFramingTransposer)
            {
                cameraDistance = (componentBase as CinemachineFramingTransposer).m_CameraDistance;
            }
        }

        public void GetMovementVariables()
        {
            if (playerStats.HasStat(EnumCollections.PlayerStats.BaseMovementSpeed))
            {
                float baseSpeed = playerStats.GetStatValue(EnumCollections.PlayerStats.BaseMovementSpeed);
                float engineSpeed = equipmentManager.GetGearStatValue(EnumCollections.ItemType.Engine, EnumCollections.Stats.MovementSpeed);
                float accesorySpeed = 0; //TODO::Get from accessories once implemented.
                currentSpeed = baseSpeed + engineSpeed + accesorySpeed;
                Acceleration = currentSpeed * AccelerationMultiplier;
                Deceleration = currentSpeed * DecelerationMultiplier;
                MaxVelocity = currentSpeed * MaxVelocityCoeficient;
                BaseSpeed = currentSpeed;
            }
        }

        private void GetBoostVariables()
        {
            if (playerStats.HasStat(EnumCollections.PlayerStats.BoostSpeed))
            {
                BoostSpeed = playerStats.GetStatValue(EnumCollections.PlayerStats.BoostSpeed);
                BoostRegen = playerStats.GetStatValue(EnumCollections.PlayerStats.BoostRegen);
                Fuel = playerStats.GetStatValue(EnumCollections.PlayerStats.BoostFuel);
            }
        }

        // Update is called once per frame
        private void Update()
        {
            UpdateAudioHooks();
            if (Time.timeScale == 0)
            {
                return;
            }
            if (inputManager.Attacking)
            {
                RotateToMouse();
            } else if (!inputManager.MovementInputZero()) { RotateToMoveDirection();}
            
            CheckEnginePS(inputManager.MovementInputZero());
            bool driftCheck = MovementInput.x <= 0.01f && MovementInput.y <= 0.01f && MovementInput.x >= -0.01f && MovementInput.y >= -0.01f ? true : false;

            //fix drift
            if (driftCheck)
            {
                MovementInput = Vector2.zero;
            }
            if(canDash && MovementInput != Vector2.zero && inputManager.GetMovementSkillButtonDown())
            {
                StartCoroutine(Dash());
            }
            Move();
        }

        private void UpdateAudioHooks()
        {
            AkSoundEngine.SetRTPCValue("PlayerShipAcceleration", MovementInput.normalized.magnitude, gameObject);
            AkSoundEngine.SetRTPCValue("PlayerShipVelocity", rb.velocity.normalized.magnitude, gameObject);
            AkSoundEngine.SetRTPCValue("PlayerShipTurnRate", currAngle, gameObject);
        }

        private IEnumerator Dash()
        {
            canDash = false;
            dashing = true;
            StartCoroutine(DashParticles());
                   
            rb.AddForce((((MovementInput * currentSpeed * DashVelocityMultiplier) * ForceModifier)), ForceMode2D.Impulse);

            yield return new WaitForSeconds(DashDuration * 0.65f);

            rb.AddForce((((MovementInput * currentSpeed * DashVelocityMultiplier) * -ForceModifier * 0.5f)), ForceMode2D.Impulse);

            yield return new WaitForSeconds(DashDuration * 0.35f);

            dashing = false;
            yield return new WaitForSeconds(DashCooldown);
            canDash = true;
        }

        private IEnumerator DashParticles()
        {
            for (int i = 0; i < 15; i++)
            {
                objectPooler.SpawnFromPool("DashParticle", transform.position, transform.rotation);
                yield return new WaitForSeconds(DashDuration / 15f);
            }
        }

        private void MoveOld()
        {
            float axisX = inputManager.GetAxisSmoothHorizontal(currentSpeed, Acceleration, Deceleration, BoostSpeed);
            float axisY = inputManager.GetAxisSmoothVertical(currentSpeed, Acceleration, Deceleration, BoostSpeed);
            
            Vector2 input = new Vector2(axisX, axisY);

            //Debug.Log("Regular: " + new Vector2(axisX,axisY) + "Normalized: " + new Vector2(axisX, axisY).normalized);
            MovementInput = input * currentSpeed * Time.deltaTime;
            //MovementInput = new Vector2(axisX, axisY) * currentSpeed * Time.deltaTime;
            transform.position += (Vector3)MovementInput;
        }

        private void Move()
        {
            Vector2 axisses = inputManager.GetAxisNormalizedCoef();

            //Debug.Log("Regular: " + new Vector2(axisX,axisY) + "Normalized: " + new Vector2(axisX, axisY).normalized);
            MovementInput = axisses * currentSpeed;

            //MovementInput = new Vector2(axisX, axisY) * currentSpeed * Time.deltaTime;
            ApplyForce();
            //transform.position += (Vector3)MovementInput;
        }

        private void ApplyForce()
        {
            if (MovementInput.x != 0)
            {
                rb.AddForce(new Vector2(MovementInput.x * ForceModifier, 0));
            }
            if (MovementInput.y != 0)
            {
                rb.AddForce(new Vector2(0, MovementInput.y * ForceModifier));
            }       
            
            if (!dashing)
            {
                float x = rb.velocity.x;
                float y = rb.velocity.y;
                x = Mathf.Clamp(rb.velocity.x, -MaxVelocity, MaxVelocity);
                y = Mathf.Clamp(rb.velocity.y, -MaxVelocity, MaxVelocity);
                rb.velocity = new Vector2(x, y);
            }
            
        }

        private void CheckEnginePS(bool input)
        {
            if(input)
            {
                foreach (ParticleSystem ps in Engines)
                {
                    if (!ps.isStopped) { ps.Stop(); }
                }
            }
            else
            {
                foreach (ParticleSystem ps in Engines)
                {
                    if (!ps.isEmitting) { ps.Play(); }
                }
            }
        }
        
        private void RotateToMouse()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            target = hit.point + (Vector3)CameraOffset.Instance.Offset * offsetMultiplier;

            var dir = target - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        }

        private void RotateToMoveDirection()
        {
            /*if (MovementInput.x <= 0.01f && MovementInput.y <= 0.01f && MovementInput.x >= -0.01f && MovementInput.y >= -0.01f && lastInputs.Count > 9)
            {
                Debug.Log(lastInputs[10]);
                target = transform.position + lastInputs[10] + (Vector3)CameraOffset.Instance.Offset * offsetMultiplier;
            }
            else {
                if (lastInputs.Count < 10) {lastInputs.Add(MovementInput); }
                else 
                { 
                    lastInputs.RemoveAt(0);
                    lastInputs.Add(MovementInput);
                }*/

            target = transform.position + (((Vector3)rb.velocity) * RotationOffsetMultiplier) + (Vector3)CameraOffset.Instance.Offset * offsetMultiplier; 
            //}
            Debug.DrawLine(transform.position, target, Color.red);
            var dir = target - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        }
    
        public float GetMovementSpeed()
        {
            return BaseSpeed;
        }
    }
}