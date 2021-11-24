namespace security.Substitution
{
    using System.Text;

    public class SubstitutionDecoder
    {
        public string Decode(string text, SubstitutionKey key)
        {
            var builder = new StringBuilder();
            foreach (var c in text)
            {
                var index = key.IndexOf(c);
                builder.Append(Utils.EnglishAlphabet[index]);
            }
            return builder.ToString();
        }
    }
}
