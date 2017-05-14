using System.Collections.Generic;
using System.Linq;

namespace Segmentus
{
    class SingleGameLogic
    {
        class GameState
        {
            public bool win = false;
            public Dictionary<int, GameState> nextWin = new Dictionary<int, GameState>();
            public Dictionary<int, GameState> nextLose = new Dictionary<int, GameState>();

            public GameState(FieldData fieldData, List<int> freeSegments)
            {
                foreach (int curID in freeSegments)
                {
                    List<int> nxtFree = new List<int>(freeSegments);
                    foreach (int excludeID in fieldData.intersectedWith[curID])
                        nxtFree.Remove(excludeID);
                    GameState nxt = new GameState(fieldData, nxtFree);
                    if (nxt.win)
                        nextWin[curID] = nxt;
                    else
                    {
                        nextLose[curID] = nxt;
                        win = true;
                    }
                }
            }
        }

        public FieldData fieldData;
        GameState curState;

        public SingleGameLogic()
        {
            fieldData = FieldDataGenerator.Generate();
            curState = new GameState(fieldData,
                new List<int>(Enumerable.Range(0, fieldData.segmentsCnt)));
        }
    }
}