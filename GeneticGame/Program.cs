namespace GeneticGame;

class Program
{
    static void Main(string[] args)
    {
        BoardUi boardUi = new BoardUi(new Engine());
        boardUi.StartGame();
    }
}