using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using VaultSave.Saver;
using VaultSave.Systems;

namespace VaultSave.Commands
{
    public class VSSaveCommand:ISaver
    {
        public void Execute<T>(T dataToSave, string path, VSSystemData vsSystemData)
        {
            string dataAsJson = JsonConvert.SerializeObject(dataToSave);
            if (vsSystemData.PrettyFormat)
            {
                dataAsJson = PrettyFormat(dataAsJson);
            }
            File.WriteAllText(path, dataAsJson);
        }

        private String PrettyFormat(string dataAsJson)
        {
            StringBuilder sb = new StringBuilder();
            int squareBracketCount = 0;
            int curlyBracketCount = 0;
            bool isInString = false;
            for (int i = 0; i < dataAsJson.Length; i++)
            {
                char c = dataAsJson[i];
                if (c == '\"')
                {
                    isInString = !isInString;
                }

                if (c == '[')
                {
                    squareBracketCount++;
                }

                if (c == ']')
                {
                    squareBracketCount--;
                }

                if (c == '{')
                {
                    curlyBracketCount++;
                }

                if (c == '}')
                {
                    curlyBracketCount--;
                }

                if (c == '{' && !isInString)
                {
                    sb.Append("{\n");
                }
                else if (c == ',' && !isInString && squareBracketCount == 0 && curlyBracketCount < 2)
                {
                    sb.Append(",\n\n");
                }
                else if (c == ',' && !isInString && squareBracketCount == 0 && curlyBracketCount >= 2)
                {
                    sb.Append(",\n");
                }

                else
                {
                    sb.Append(c);
                }

                
            }
            return sb.ToString();
        }
    }

    
}