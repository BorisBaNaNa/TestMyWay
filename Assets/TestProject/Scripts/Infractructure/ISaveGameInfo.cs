﻿using Assets.TestProject.Scripts.Data;

namespace Assets.TestProject.Scripts.Infractructure
{
    public interface ISaveGameInfo
    {
        void SaveInfoTo(GameInfo gameInfo);
    }
}