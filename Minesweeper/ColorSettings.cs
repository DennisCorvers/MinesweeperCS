using Minesweeper.Properties;
using System.Drawing;

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
        private static readonly Color[] TextColors = new Color[9]
        {
            ColorTranslator.FromHtml("#FF00FF"),
            ColorTranslator.FromHtml("#0100FD"),
            ColorTranslator.FromHtml("#017F01"),
            ColorTranslator.FromHtml("#F90400"),
            ColorTranslator.FromHtml("#01007F"),
            ColorTranslator.FromHtml("#890003"),
            ColorTranslator.FromHtml("#027F80"),
            ColorTranslator.FromHtml("#000000"),
            ColorTranslator.FromHtml("#808080")
        };

        /// <summary>
        /// Color of a default square.
        /// </summary>
        public static Color DefaultSQ
            => Color.White;
        /// <summary>
        /// Color of a clicked, visible square.
        /// </summary>
        public static Color ClickedSQ
            => ColorTranslator.FromHtml("#BDBEBC");
        /// <summary>
        /// Color of a marked square.
        /// </summary>
        public static Color MarkedSQ
            => Color.PaleVioletRed;

        /// <summary>
        /// Determines which color should match the given numeric value.
        /// </summary>
        /// <param name="Number">The number of surrounding mines.</param>
        /// <returns></returns>
        public static Color GetColor(int Number)
        {
            if (Number < 1 || Number > 8)
                return TextColors[0];

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

        public static Font ButtonFont = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold);

    }
}
