using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleControls
{
    public class Button : Control
    {
        public string Label { get; set; } = "";
        public override int Height => 1;

        public override void Redraw()
        {
            if (IsFocused)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            base.Redraw();
            string labelToDraw = Label.Substring(0, Math.Min(Label.Length, Width));
            Console.SetCursorPosition(AbsoluteX + (Width - labelToDraw.Length) / 2, AbsoluteY);
            Console.Write(labelToDraw);
            Console.ResetColor();
        }

        public override bool HandleKey(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Enter)
            {
                Click?.Invoke(this, null);
                return true;
            }
            return false;
        }

        public event EventHandler<object> Click;
    }
}
