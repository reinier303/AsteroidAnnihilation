/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID PLAY_DEFAULTMUSIC = 1857418338U;
        static const AkUniqueID PLAY_PLAYERSHIPENGINE = 1672365273U;
        static const AkUniqueID PLAY_PLAYERSHIPSHOOTGUN = 1172773070U;
        static const AkUniqueID PLAY_SHIPDASH = 3482721360U;
        static const AkUniqueID PLAY_SHIPPLASMAGUNIMPACT = 4264920734U;
        static const AkUniqueID PLAY_SPACEAMBIENCE = 915492624U;
        static const AkUniqueID STOP_PLAYERSHIPENGINE = 4236749427U;
        static const AkUniqueID TEMP_PLAY_ETFX_SHOOT_LASER = 3201422434U;
        static const AkUniqueID TEMP_PLAY_LEVELUP = 1450272620U;
        static const AkUniqueID TEMP_PLAY_PLASMAGUNSHOT1 = 3965960414U;
        static const AkUniqueID TEMP_PLAY_PLASMAGUNSHOT2 = 3965960413U;
        static const AkUniqueID TEMP_PLAY_PLASMAGUNSHOT3 = 3965960412U;
        static const AkUniqueID TEMP_PLAY_PLASMAGUNSHOT4 = 3965960411U;
        static const AkUniqueID TEMP_PLAY_SWARMEXPLOSION = 2360230854U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace GAMESTATE
        {
            static const AkUniqueID GROUP = 4091656514U;

            namespace STATE
            {
                static const AkUniqueID CRAFTINGMENU = 1247366870U;
                static const AkUniqueID INVENTORYMENU = 2407792274U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID PAUSED = 319258907U;
                static const AkUniqueID STARTMENU = 3944636910U;
            } // namespace STATE
        } // namespace GAMESTATE

    } // namespace STATES

    namespace SWITCHES
    {
        namespace NUMBEROFENEMIES
        {
            static const AkUniqueID GROUP = 321019979U;

            namespace SWITCH
            {
                static const AkUniqueID AFEWENEMIES = 1938642156U;
                static const AkUniqueID GIANTSWARM = 1361491240U;
                static const AkUniqueID MANYENEMIES = 2915076976U;
                static const AkUniqueID NOENEMIES = 2111510692U;
            } // namespace SWITCH
        } // namespace NUMBEROFENEMIES

        namespace PLAYERSHIPGUNLEVELSWITCH
        {
            static const AkUniqueID GROUP = 1749199752U;

            namespace SWITCH
            {
                static const AkUniqueID LEVEL_01 = 987635873U;
            } // namespace SWITCH
        } // namespace PLAYERSHIPGUNLEVELSWITCH

        namespace PLAYERSHIPHEALTHSTATES
        {
            static const AkUniqueID GROUP = 155164948U;

            namespace SWITCH
            {
                static const AkUniqueID ALMOSTDEAD = 777824873U;
                static const AkUniqueID DEAD = 2044049779U;
                static const AkUniqueID HEALTHALMOSTFULL = 3493536634U;
                static const AkUniqueID HEALTHFULL = 4197849920U;
                static const AkUniqueID HEALTHLOW = 1924267063U;
                static const AkUniqueID HEALTHMEDIUM = 825447798U;
            } // namespace SWITCH
        } // namespace PLAYERSHIPHEALTHSTATES

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID NUMBEROFENEMIES = 321019979U;
        static const AkUniqueID PLAYERSHIPACCELERATION = 1918122584U;
        static const AkUniqueID PLAYERSHIPENERGY = 2508725640U;
        static const AkUniqueID PLAYERSHIPGUNLEVEL = 2663975140U;
        static const AkUniqueID PLAYERSHIPHEALTH = 1438366550U;
        static const AkUniqueID PLAYERSHIPTURNRATE = 1347618923U;
        static const AkUniqueID PLAYERSHIPVELOCITY = 3228488565U;
    } // namespace GAME_PARAMETERS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
