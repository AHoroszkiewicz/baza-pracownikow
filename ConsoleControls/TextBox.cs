using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleControls
{
    public class TextBox : BoxedControl
    {
        public override int Height => 3;


        public virtual string Text { get; set; } = "";

        public virtual int CursorPosition { get; set; } = 0;
        public virtual int ScrollOffset { get; set; } = 0;
        public virtual int ScrollWindow => Width - 2;

        public override void Redraw()
        {
            base.Redraw();
            Console.SetCursorPosition(AbsoluteX + 1, AbsoluteY + 1);
            string textToDraw = Text.Substring(ScrollOffset, Math.Min(Text.Length - ScrollOffset, ScrollWindow));
            Console.Write(textToDraw);
            Console.SetCursorPosition(AbsoluteX + 1 + CursorPosition - ScrollOffset, AbsoluteY + 1);
        }

        public void Reset()
        {
            Text = "";
            CursorPosition = 0;
        }

        public override void GainedFocus()
        {
            Console.CursorVisible = true;
        }

        public override void LostFocus()
        {
            Console.CursorVisible = false;
        }

        public event EventHandler<string> TextAcceptedHandler;

        private void TextAccepted(object sender, string text)
        {
            TextAcceptedHandler?.Invoke(this, text);
            if (TextAcceptedHandler != null)
            {
                Text = "";
                CursorPosition = 0;
                Redraw();
            }
        }

        public override bool HandleKey(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.LeftArrow)
            {
                CursorPosition--;
                if (CursorPosition < 0)
                    CursorPosition = 0;
                if (CursorPosition < ScrollOffset)
                {
                    ScrollOffset--;
                    Redraw();
                }
                Console.SetCursorPosition(AbsoluteX + 1 + CursorPosition - ScrollOffset, AbsoluteY + 1);
                return true;
            }
            if (keyInfo.Key == ConsoleKey.RightArrow)
            {
                CursorPosition++;
                if (CursorPosition > Text.Length)
                    CursorPosition = Text.Length;
                if (CursorPosition - ScrollOffset >= ScrollWindow)
                {
                    ScrollOffset++;
                    Redraw();
                }
                Console.SetCursorPosition(AbsoluteX + 1 + CursorPosition - ScrollOffset, AbsoluteY + 1);
                return true;
            }
            if (keyInfo.Key == ConsoleKey.Backspace)
            {
                if (CursorPosition > 0)
                {
                    Text = Text.Remove(CursorPosition - 1, 1);
                    CursorPosition--;
                    if (CursorPosition < ScrollOffset)
                    {
                        ScrollOffset--;
                        Redraw();
                    }
                    else
                    {
                        Redraw();
                    }
                }
                Console.SetCursorPosition(AbsoluteX + 1 + CursorPosition - ScrollOffset, AbsoluteY + 1);
                return true;
            }
            if (keyInfo.Key == ConsoleKey.Delete)
            {
                if (CursorPosition < Text.Length)
                {
                    Text = Text.Remove(CursorPosition, 1);
                    Redraw();
                    Console.SetCursorPosition(AbsoluteX + 1 + CursorPosition - ScrollOffset, AbsoluteY + 1);
                    return true;
                }
            }
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                TextAccepted(this, Text);
                return true;
            }
            if (keyInfo.KeyChar != '\0')
            {
                Text = Text.Insert(CursorPosition, keyInfo.KeyChar.ToString());
                CursorPosition++;
                if (CursorPosition - ScrollOffset >= ScrollWindow)
                {
                    ScrollOffset++;
                    Redraw();
                }
                else
                {
                    Redraw();
                }
                return true;
            }
            return false;
        }
    }
}
