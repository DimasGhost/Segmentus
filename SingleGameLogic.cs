using System.Collections.Generic;
using System.Linq;
using System;

namespace Segmentus
{
    class SingleGameLogic
    {
        const int StartGameDelay = 1300;
        const int BotThinkingDuration = 1000;
        const int BotMovingDuration = 1000;
        public enum GameStatus { Empty, PlayersTurn, BotsTurn, Win, Lose };
        class GameState
        {
            const int MaxMoves = 6;
            public double[] diagnoses = new double[MaxMoves];
            public Dictionary<int, GameState> next = new Dictionary<int, GameState>();

            public GameState(FieldData fieldData, List<int> freeSegments)
            {
                diagnoses[0] = 0.5;
                for (int i = 1; i < MaxMoves; ++i)
                    diagnoses[i] = 0;
                if (freeSegments.Count == 0)
                    return;
                foreach (int curID in freeSegments)
                {
                    List<int> nxtFree = new List<int>(freeSegments);
                    foreach (int excludeID in fieldData.intersectedWith[curID])
                        nxtFree.Remove(excludeID);
                    next[curID] = new GameState(fieldData, nxtFree);
                    for (int i = 1; i < MaxMoves; ++i)
                        diagnoses[i] += 1 - next[curID].diagnoses[i - 1];
                }
                for (int i = 1; i < MaxMoves; ++i)
                    diagnoses[i] /= next.Count();
            }
        }

        public static int botDepth;
        static Random random = new Random();

        public FieldData fieldData;
        GameState curState;
        public event Action<GameStatus> StatusChanged;
        public event Action<int> BotMoved;

        public SingleGameLogic()
        {
            fieldData = FieldDataGenerator.Generate();
            curState = new GameState(fieldData,
                new List<int>(Enumerable.Range(0, fieldData.segmentsCnt)));
        }

        void DelayAction(Action action, int duration)
        {
            HandyAnimator delay = HandyAnimator.OfNothing(duration);
            delay.After += action;
            delay.core.Start();
        }

        public void OnPlayersMove(int segID)
        {
            curState = curState.next[segID];
            if (curState.next.Count == 0)
            {
                StatusChanged?.Invoke(GameStatus.Win);
                return;
            }
            StatusChanged?.Invoke(GameStatus.BotsTurn);
            DelayAction(MakeBotMove, BotThinkingDuration);
        }

        void MakeBotMove()
        {
            double minProb = 2;
            List<int> segIDs = new List<int>();
            foreach (int segID in curState.next.Keys)
            {
                double curProb = curState.next[segID].diagnoses[botDepth];
                if (curProb < minProb)
                {
                    segIDs.Clear();
                    minProb = curProb;
                }
                if (Math.Abs(curProb - minProb) < double.Epsilon)
                    segIDs.Add(segID);
            }
            int chosenSegID = segIDs[random.Next(segIDs.Count)];
            BotMoved?.Invoke(chosenSegID);
            curState = curState.next[chosenSegID];
            GameStatus nextStatus;
            if (curState.next.Count == 0)
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