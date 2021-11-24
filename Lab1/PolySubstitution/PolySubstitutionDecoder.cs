namespace security.PolySubstitution
{
    using System.Text;

    public class PolySubstitutionDecoder
    {
        public string Decode(string text, PolySubstitutionKey key)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];
                var k = key[i % (key.Length / Utils.NumberOfLetters)];
                var index = k.IndexOf(c);
                builder.Append(Utils.EnglishAlphabet[index]);
            }
            return builder.ToString();
        }
    }
}
