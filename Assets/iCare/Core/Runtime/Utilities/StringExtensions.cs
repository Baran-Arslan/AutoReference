namespace iCare.Utilities {
    public static class StringExtensions {
        public static string SetColor(this string text, string color) {
            return $"<color={color}>{text}</color>";
        }

        public static string SetSize(this string text, int size) {
            return $"<size={size}>{text}</size>";
        }

        public static string SetBold(this string text) {
            return $"<b>{text}</b>";
        }

        public static string Highlight(this string text) {
            return text.SetBold().SetColor("red").SetSize(14);
        }
    }
}