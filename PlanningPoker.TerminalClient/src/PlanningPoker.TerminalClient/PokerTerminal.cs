using PlanningPoker.Client.Model;
using PlanningPoker.TerminalClient.Input;
using System;
using System.Threading.Tasks;

namespace PlanningPoker.TerminalClient
{
    public class PokerTerminal
    {
        public PokerTerminal()
        {
        }

        private void OutputProgramHeader()
        {
            Console.WriteLine("\n" +
                 "'########::'##::::::::::'###::::'##::: ##:'##::: ##:'####:'##::: ##::'######:::\n" +
                 " ##.... ##: ##:::::::::'## ##::: ###:: ##: ###:: ##:. ##:: ###:: ##:'##... ##::\n" +
                 " ##:::: ##: ##::::::::'##:. ##:: ####: ##: ####: ##:: ##:: ####: ##: ##:::..:::\n" +
                 " ########:: ##:::::::'##:::. ##: ## ## ##: ## ## ##:: ##:: ## ## ##: ##::'####:\n" +
                 " ##.....::: ##::::::: #########: ##. ####: ##. ####:: ##:: ##. ####: ##::: ##::\n" +
                 " ##:::::::: ##::::::: ##.... ##: ##:. ###: ##:. ###:: ##:: ##:. ###: ##::: ##::\n" +
                 " ##:::::::: ########: ##:::: ##: ##::. ##: ##::. ##:'####: ##::. ##:. ######:::\n" +
                 "..:::::::::........::..:::::..::..::::..::..::::..::....::..::::..:::......::::");
            Console.WriteLine(
                "::::::::::::::::::::::::::'########:::'#######::'##:::'##:'########:'########::\n" +
                ":::::::::::::::::::::::::: ##.... ##:'##.... ##: ##::'##:: ##.....:: ##.... ##:\n" +
                ":::::::::::::::::::::::::: ##:::: ##: ##:::: ##: ##:'##::: ##::::::: ##:::: ##:\n" +
                ":::::::::::::::::::::::::: ########:: ##:::: ##: #####:::: ######::: ########::\n" +
                ":::::::::::::::::::::::::: ##.....::: ##:::: ##: ##. ##::: ##...:::: ##.. ##:::\n" +
                ":::::::::::::::::::::::::: ##:::::::: ##:::: ##: ##:. ##:: ##::::::: ##::. ##::\n" +
                ":::::::::::::::::::::::::: ##::::::::. #######:: ##::. ##: ########: ##:::. ##:\n" +
                "::::::::::::::::::::::::::..::::::::::.......:::..::::..::........::..:::::..::");
        }
        private void OutputShortProgramHeader()
        {
            Console.WriteLine(":::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::");
            Console.WriteLine("::::::::::::::::: PLANNING ::::::::::::::::::::::::::::: POKER:::::::::::::::::");
            Console.WriteLine(":::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::\n\n");
        }

        public virtual Task<StartMenuOption> GetStartOption()
        {
            OutputProgramHeader();

            Console.WriteLine("\n\nWelcome to planning poker. Please choose from the options below.");
            Console.WriteLine("\n1. Start new poker session\n2. Join an existing poker session\nEnter option: ");

            StartMenuOption selectedOption = StartMenuOption.StartSession;
            var optionNotChosen = true;
            while (optionNotChosen)
            {
                var key = Console.ReadKey(true);
                switch (key.KeyChar)
                {
                    case '1':
                        selectedOption = StartMenuOption.StartSession;
                        optionNotChosen = false;
                        break;
                    case '2':
                        selectedOption = StartMenuOption.JoinSession;
                        optionNotChosen = false;
                        break;
                    default:
                        Console.WriteLine("Selected option is invalid. Please chose a valid option");
                        break;
                }
            }
            return Task.FromResult(selectedOption);
        }
        public virtual Task Output(string outputText)
        {
            Console.WriteLine(outputText);
            return Task.CompletedTask;
        }

        public virtual Task ClearOutput()
        {
            Console.Clear();
            return Task.CompletedTask;
        }

        public virtual Task<string> GetStringInput()
        {
            return Task.FromResult(Console.ReadLine());
        }
        public virtual Task RenderStatusInformation(PokerSession sessionInformation)
        {
            Console.Clear();
            OutputShortProgramHeader();

            Console.WriteLine($"Current session: {sessionInformation.SessionId}");
            foreach (var user in sessionInformation.Participants)
            {
                Console.WriteLine($"{user.Name} - {user.CurrentVoteDescription}");
            }

            return Task.CompletedTask;
        }
        public virtual Task<(string sessionId, string userName)> GetUserJoinInformation()
        {
            Console.Clear();
            OutputShortProgramHeader();

            Console.WriteLine("JOIN SESSION\n");
            Console.WriteLine("Please enter your name and hit return:");
            var userName = Console.ReadLine();

            Console.WriteLine("Input the ID of the session to join and hit return:");
            var sessionId = Console.ReadLine();

            Console.WriteLine("Please wait...");

            return Task.FromResult((sessionId, userName));
        }
        public virtual Task HandleSessionEnded()
        {
            Console.WriteLine("*****");
            Console.WriteLine("The session ended or you were removed from the session");
            Console.WriteLine("\nPress enter to continue...");
            Console.ReadLine();
            Console.Clear();
            return Task.CompletedTask;
        }
        private Task RenderInSessionOptions(PokerSession sessionInformation)
        {
            //if(sessionInformation.StoryPointType)
            throw new NotImplementedException();
        }
    }
}
