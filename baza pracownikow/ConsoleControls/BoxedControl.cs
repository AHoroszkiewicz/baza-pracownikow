using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleControls
{
    public class BoxedControl : Control
    {
        public const int TOP = 1;
        public const int BOTTOM = 2;
        public const int LEFT = 4;
        public const int RIGHT = 8;

        public const int SINGLE = 0;
        public const int DOUBLE = 16;

        public const int LABEL = 32;

        public static Dictionary<int, char> BoxCharacters = new Dictionary<int, char>()
        {
            { TOP | SINGLE, '─' },
            { BOTTOM | SINGLE, '─' },
            { LEFT | SINGLE, '│' },
            { RIGHT | SINGLE, '│' },
            { TOP | LEFT | SINGLE, '┌' },
            { TOP | RIGHT | SINGLE, '┐' },
            { BOTTOM | LEFT | SINGLE, '└' },
            { BOTTOM | RIGHT | SINGLE, '┘' },
            { LABEL | LEFT | SINGLE, '┤'},
            { LABEL | RIGHT | SINGLE, '├' },
            { TOP |  DOUBLE, '═' },
            { BOTTOM | DOUBLE, '═' },
            { LEFT |  DOUBLE, '║' },
            { RIGHT | DOUBLE, '║' },
            { TOP | LEFT | DOUBLE, '╔' },
            { TOP | RIGHT | DOUBLE, '╗' },
            { BOTTOM | LEFT | DOUBLE, '╚' },
            { BOTTOM | RIGHT | DOUBLE, '╝' },
            { LABEL | LEFT | DOUBLE, '╣'},
            { LABEL | RIGHT | DOUBLE, '╠' },
        };

        public int BorderModifier => IsFocused ? DOUBLE : SINGLE;

        public override void Redraw()
        {
            base.Redraw();
            DrawBox();
        }

        public string Label { get; set; } = "";

        private void DrawBox()
        {
            Console.SetCursorPosition(AbsoluteX, AbsoluteY);
            Console.Write(BoxCharacters[TOP | LEFT | BorderModifier]);
            if (Width > 2)
                Console.Write(new string(BoxCharacters[TOP | BorderModifier], Width - 2));
            Console.Write(BoxCharacters[TOP | RIGHT | BorderModifier]);
            Console.SetCursorPosition(AbsoluteX, AbsoluteY + Height - 1);
            Console.Write(BoxCharacters[BOTTOM | LEFT | BorderModifier]);
            if (Width > 2)
                Console.Write(new string(BoxCharacters[BOTTOM | BorderModifier], Width - 2));
            Console.Write(BoxCharacters[BOTTOM | RIGHT | BorderModifier]);
            for (int y = AbsoluteY + 1; y < AbsoluteY + Height - 1; y++)
            {
                Console.SetCursorPosition(AbsoluteX, y);
                Console.Write(BoxCharacters[LEFT | BorderModifier]);
                Console.SetCursorPosition(AbsoluteX + Width - 1, y);
                Console.Write(BoxCharacters[RIGHT | BorderModifier]);
            }
            if (Label.Length > 0)
            {
                Console.SetCursorPosition(AbsoluteX + 2, AbsoluteY);
                Console.Write(BoxCharacters[LABEL | LEFT | BorderModifier]);
                Console.Write(Label);
                Console.Write(BoxCharacters[LABEL | RIGHT | BorderModifier]);
            }
        }
    }
}
