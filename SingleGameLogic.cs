using System.Collections.Generic;
using System;
using Android.Util;

namespace Segmentus
{
    class SingleGameLogic
    {
        const int StartGameDelay = 1300;
        const int BotThinkingDuration = 1000;
        const int BotMovingDuration = 1000;

        public enum GameStatus { Empty, PlayersTurn, BotsTurn, Win, Lose };

        const int MaxBotDepth = 4;
        Dictionary<Bitmask128, float[]> statePredictions = 
            new Dictionary<Bitmask128, float[]>();
        Bitmask128 curStateMask;
        Bitmask128[] segmentProhibitMask;

        public static int botDepth;
        static Random random = new Random();

        public FieldData fieldData;
        public event Action<GameStatus> StatusChanged;
        public event Action<int> BotMoved;

        int cnt = 0;

        float[] MakeStatePredictions(Bitmask128 mask)
        {
            ++cnt;
            if (cnt % 1000 == 0)
                Log.Info("kek", cnt.ToString());
            float[] curPred = statePredictions[mask] = new float[MaxBotDepth + 1];
            curPred[0] = 0.5f;
            for (int i = 1; i <= MaxBotDepth; ++i)
                curPred[i] = 0;
            int c = 0;
            for (int i = 0; i < fieldData.segmentsCnt; ++i)
            {
                if (!mask[i])
                    continue;
                ++c;
                Bitmask128 nxt = mask & segmentProhibitMask[i];
                float[] nxtPred;
                if (!statePredictions.ContainsKey(nxt))
                    nxtPred = MakeStatePredictions(nxt);
                else
                    nxtPred = statePredictions[nxt];
                for (int j = 1; j <= MaxBotDepth; ++j)
                    curPred[j] += 1 - nxtPred[j - 1];
            }
            for (int i = 1; i <= MaxBotDepth; ++i)
                curPred[i] /= c;
            return curPred;
        }

        public SingleGameLogic()
        {
            fieldData = FieldDataGenerator.Generate();
            curStateMask = new Bitmask128(fieldData.segmentsCnt);
            segmentProhibitMask = new Bitmask128[fieldData.segmentsCnt];
            for (int i = 0; i < fieldData.segmentsCnt; ++i)
            {
                segmentProhibitMask[i] = curStateMask;
                foreach (int badSegID in fieldData.intersectedWith[i])
                    segmentProhibitMask[i][badSegID] = false;
            }
            statePredictions[Bitmask128.Zero] = new float[MaxBotDepth + 1];
            statePredictions[Bitmask128.Zero][0] = 0.5f;
            for (int i = 1; i <= MaxBotDepth; ++i)
                statePredictions[Bitmask128.Zero][i] = 0;
            Log.Info("kek", "start");
            MakeStatePredictions(curStateMask);
        }

        void DelayAction(Action action, int duration)
        {
            HandyAnimator delay = HandyAnimator.OfNothing(duration);
            delay.After += action;
            delay.core.Start();
        }

        public void OnPlayersMove(int segID)
        {
            curStateMask &= segmentProhibitMask[segID];
            if (curStateMask.Equals(Bitmask128.Zero))
            {
                StatusChanged?.Invoke(GameStatus.Win);
                return;
            }
            StatusChanged?.Invoke(GameStatus.BotsTurn);
            DelayAction(MakeBotMove, BotThinkingDuration);
        }

        void MakeBotMove()
        {
            float minProb = 2;
            List<int> segIDs = new List<int>();
            for (int i = 0; i < fieldData.segmentsCnt; ++i)
            {
                if (!curStateMask[i])
                    continue;
                Bitmask128 nxt = curStateMask & segmentProhibitMask[i];
                float curProb = statePredictions[nxt][botDepth];
                if (curProb < minProb)
                {
                    segIDs.Clear();
                    minProb = curProb;
                }
                if (Math.Abs(curProb - minProb) < float.Epsilon)
                    segIDs.Add(i);
            }
            int chosenSegID = segIDs[random.Next(segIDs.Count)];
            BotMoved?.Invoke(chosenSegID);
            curStateMask &= segmentProhibitMask[chosenSegID];
            GameStatus nextStatus;
            if (curStateMask.Equals(Bitmask128.Zero))
                nextStatus = GameStatus.Lose;
            else
                nextStatus = GameStatus.PlayersTurn;
            DelayAction(() => StatusChanged?.Invoke(nextStatus), BotMovingDuration);
        }

        public void StartGame()
        {
            GameStatus nextStatus;
            if (random.Next(500) < 250)
                nextStatus = GameStatus.PlayersTurn;
            else
            {
                nextStatus = GameStatus.BotsTurn;
                DelayAction(MakeBotMove, StartGameDelay + BotThinkingDuration);
            }
            DelayAction(() => StatusChanged?.Invoke(nextStatus), StartGameDelay);
        }

        public void ClearEvents()
        {
            BotMoved = null;
            StatusChanged = null;
        }
    }
}