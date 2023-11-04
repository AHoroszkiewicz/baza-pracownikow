namespace ConsoleControls
{
    public class Control
    {
        public virtual int X { get; set; }
        public virtual int Y { get; set; }

        public virtual int Width { get; set; }
        public virtual int Height { get; set; }

        public virtual bool IsFocused => Parent == null? false :  Parent?.FocusedChild == this;

        private Control? _parent;

        public virtual Control? Parent 
        {
            get
            {
                return _parent;
            }
            set
            {
                if (_parent != null && _parent.Children.Contains(this))
                {
                    _parent.Children.Remove(this);
                }
                _parent = value;
                if (_parent != null && !_parent.Children.Contains(this))
                {
                    _parent.Children.Add(this);
                }
            }
        }
        public virtual List<Control> Children { get; set; } 
            = new List<Control>();

        public virtual List<Control> UnfocusableChildren { get; set; } = new List<Control>();

        public virtual int FocusedChildIndex { get; set; } = 0;
        public virtual Control? FocusedChild
        {
            get => FocusedChildIndex >= Children.Count ? null : Children[FocusedChildIndex];
            set
            {
                FocusedChild?.LostFocus();
                var lost = FocusedChild;
                FocusedChildIndex = value != null ? Children.IndexOf(value) : 0;
                FocusedChild?.GainedFocus();
                if (lost != FocusedChild)
                {
                    lost?.Redraw();
                    FocusedChild?.Redraw();
                }
            }
        } 

        public virtual int AbsoluteX => Parent?.AbsoluteX + X ?? X;
        public virtual int AbsoluteY => Parent?.AbsoluteY + Y ?? Y;
        public virtual ConsoleKey FocusSwitcher { get; set; } = ConsoleKey.Tab;

        public virtual void Redraw()
        {
            CleanUp();
            foreach (var child in Children)
            {
                child.Redraw();
            }
            foreach (var child in UnfocusableChildren)
            {
                child.Redraw();
            }
        }

        public virtual void CleanUp()
        {
            for (int y = AbsoluteY; y < AbsoluteY + Height; y++)
            {
                Console.SetCursorPosition(AbsoluteX, y);
                Console.Write(new string(' ', Width));
            }
        }

        public virtual void LostFocus()
        {

        }

        public virtual void GainedFocus()
        {

        }

        public virtual bool HandleKey(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == FocusSwitcher)
            {
                if (Children.Count > 0)
                {
                    if (!keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift))
                    {
                        var previouslyFocusedChild = FocusedChild;
                        FocusedChildIndex++;
                        FocusedChildIndex %= Children.Count;
                        previouslyFocusedChild?.Redraw();
                        previouslyFocusedChild?.LostFocus();
                        if (Children.Count > 1)
                            FocusedChild?.Redraw();
                        FocusedChild?.GainedFocus();
                        return true;
                    }
                    else
                    {
                        var previouslyFocusedChild = FocusedChild;
                        FocusedChildIndex--;
                        if (FocusedChildIndex < 0)
                            FocusedChildIndex = Children.Count - 1;
                        previouslyFocusedChild?.Redraw();
                        previouslyFocusedChild?.LostFocus();
                        if (Children.Count > 1)
                            FocusedChild?.Redraw();
                        FocusedChild?.GainedFocus();
                        return true;
                    }
                }
            }
            return FocusedChild?.HandleKey(keyInfo) ?? false;
        }
    }
}