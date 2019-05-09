using System;

namespace Ex02_Othelo
{
    class Game
    {
        private string m_FirstUser;

        public Game()
        {
            Controller.ShowMessage(string.Format(
@"Welcome to Othelo!
Please Write your Name and then press Enter:"
));
            m_FirstUser = Controller.GetInputFromUser();

 
        }
    }
}
