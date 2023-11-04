using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleControls
{
    public class ListBox : BoxedControl
    {
        private class ListItem : Control
        {
            public object Item { get; set; }

            public override int X => 1;
            public override int Y => 1 + Parent.Children.IndexOf(this) - (Parent as ListBox).ScrollOffset;
            public override int Width => Parent.Width - 2;
            public override int Height => 1;

            public override void Redraw()
            {
                if (IsFocused)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                base.Redraw();
                Console.SetCursorPosition(AbsoluteX, AbsoluteY);
                string stringItemInterpretation = Item.ToString();
                stringItemInterpretation = stringItemInterpretation.Substring(0, Math.Min(stringItemInterpretation.Length, Width));
                Console.Write(stringItemInterpretation);
                Console.ResetColor();
            }
        }

        public virtual int ScrollOffset { get; set; } = 0;
        public virtual int ScrollWindow => Height - 2;

        public virtual object? SelectedItem => (FocusedChild as ListItem)?.Item;
        public virtual IEnumerable<object> AllItems => Children.Select(child => (child as ListItem)?.Item);

        public override bool HandleKey(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                var previouslyFocusedChild = FocusedChild;
                FocusedChildIndex++;
                if (FocusedChildIndex >= Children.Count)
                    FocusedChildIndex = Children.Count - 1;

                previouslyFocusedChild?.Redraw();
                if (Children.Count > 1)
                    FocusedChild?.Redraw();
                if (FocusedChildIndex - ScrollOffset >= ScrollWindow)
                {
                    ScrollOffset++;
                    Redraw();
                }
                return true;
            }
            if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                var previouslyFocusedChild = FocusedChild;
                FocusedChildIndex--;
                if (FocusedChildIndex < 0)
                    FocusedChildIndex = 0;

                previouslyFocusedChild?.Redraw();
                if (Children.Count > 1)
                    FocusedChild?.Redraw();
                if (FocusedChildIndex - ScrollOffset < 0)
                {
                    ScrollOffset--;
                    Redraw();
                }
                return true;
            }
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                ItemPressed?.Invoke(this, (FocusedChild as ListItem)?.Item);
                return true;
            }
            return FocusedChild?.HandleKey(keyInfo) ?? false;
        }

        public void AddItem(object item)
        {
            var listItem = new ListItem() { Item = item, Parent = this };
        }

        public void RemoveItem(object item)
        {
            var listItem = Children.FirstOrDefault(child => (child as ListItem)?.Item == item);
            if (listItem != null)
            {
                Children.Remove(listItem);
            }
        } 

        public override void Redraw()
        {
            base.Redraw();
            DrawItems();
        }

        private void DrawItems()
        {
            for (int i = ScrollOffset; i < ScrollOffset + ScrollWindow && i < Children.Count; i++)
            {
                var child = Children[i];
                child.Redraw();
            }
        }

        public event EventHandler<object> ItemPressed;
    }
}
