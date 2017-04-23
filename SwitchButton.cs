using System;
using Android.Graphics;

namespace Segmentus
{
    class SwitchButton : Button
    {
        public event Action<int> StateChanged;
        int state;
        public int State
        {
            get { return state; }
            set
            {
                state = value % faces.Length;
                face = faces[state];
                StateChanged?.Invoke(state);
                OnAppearanceChanged();
            }
        }
        DrawablePart[] faces;

        public SwitchButton(int defaultState, DrawablePart[] faces, Rect localBounds, 
            Pivot parentPivot, float x = 0, float y = 0) : 
            base(faces[defaultState], localBounds, parentPivot, x, y)
        {
            state = defaultState;
            this.faces = faces;
            Pressed += () => ++State;
        }
    }
}