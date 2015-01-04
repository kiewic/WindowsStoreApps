using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace Chopsticks
{
    class PasscodeManager
    {
        private const string PasscodeKey = "Passcode";

        private static string passcode;

        public static string Passcode
        {
            get
            {
                if (String.IsNullOrEmpty(passcode))
                {
                    LoadOrCreatePasscode();
                }
                return passcode;
            }
        }

        private static void LoadOrCreatePasscode()
        {
            // TODO: Get passcode from local settings.
            IPropertySet localValues = ApplicationData.Current.LocalSettings.Values;
            string passcode = localValues[PasscodeKey] as string;

            if (String.IsNullOrEmpty(passcode))
            {
                GenerateNewPasscode();
                localValues[PasscodeKey] = passcode;
            }
        }

        public static void GenerateNewPasscode()
        {
            Random random = new Random();
            StringBuilder builder = new StringBuilder(7);

            // There are 26 letters, so get 3 random letters.
            for (int i = 0; i < 3; i++)
            {
                int offset = random.Next(0, 25);
                char randomLetter = (char)('A' + offset);
                builder.Append(randomLetter);
            }

            // Get 4 random numbers between 1-9, do not include zero to avoid confusion with letter O.
            for (int i = 0; i < 4; i++)
            {
                int randomNumber = random.Next(0, 8) + 1;
                builder.Append(randomNumber);
            }

            passcode = builder.ToString();
        }
    }
}
