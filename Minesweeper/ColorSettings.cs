using Minesweeper.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    /// <summary>
    /// Static settings that determine colors etc.
    /// </summary>
    public static class ColorSettings
    {
        /// <summary>
        /// List of colors matching numeric values of surrounding mines.
        /// </summary>
        private static Dictionary<int, Color> TextColors = new Dictionary<int, Color>()
        {
            {1, ColorTranslator.FromHtml("#0100FD") },
            {2, ColorTranslator.FromHtml("#017F01") },
            {3, ColorTranslator.FromHtml("#F90400") },
            {4, ColorTranslator.FromHtml("#01007F") },
            {5, ColorTranslator.FromHtml("#890003") },
            {6, ColorTranslator.FromHtml("#027F80") },
            {7, ColorTranslator.FromHtml("#000000") },
            {8, ColorTranslator.FromHtml("#808080") }
        };

        /// <summary>
        /// Color of a default square.
        /// </summary>
        public static Color DefaultSQ
        { get { return Color.White; } }
        /// <summary>
        /// Color of a clicked, visible square.
        /// </summary>
        public static Color ClickedSQ
        { get { return ColorTranslator.FromHtml("#BDBEBC"); } }
        /// <summary>
        /// Color of a marked square.
        /// </summary>
        public static Color MarkedSQ
        { get { return Color.PaleVioletRed; } }

        /// <summary>
        /// Determines which color should match the given numeric value.
        /// </summary>
        /// <param name="Number">The number of surrounding mines.</param>
        /// <returns></returns>
        public static Color GetColor(int Number)
        {
            if (!TextColors.ContainsKey(Number))
            { return Color.Black; }

            return TextColors[Number];
        }
        /// <summary>
        /// Image of a flag for marking mines.
        /// </summary>
        public static Image FlagImage = Resources.flag;
        /// <summary>
        /// Image of a mine.
        /// </summary>
        public static Image MineImage = Resources.mine;
        /// <summary>
        /// Image of a questionmark.
        /// </summary>
        public static Image QuestionMarkImage = Resources.Questionmark;

    }
}
