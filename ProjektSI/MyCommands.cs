using System.Windows.Input;

namespace ProjektSI
{
    class MyCommands
    {
        public static RoutedUICommand SetStartNode;
        public static RoutedUICommand SetEndNode;
        public static RoutedUICommand CleanLabyrinth;
        public static RoutedUICommand ResetLabyrinth;
        public static RoutedUICommand InvertLabyrinth;

        public static RoutedUICommand StartAlghoritm;
        public static RoutedUICommand StopAlghoritm;
        public static RoutedUICommand SaveAlghoritm;
        public static RoutedUICommand LoadAlghoritm;
        public static RoutedUICommand Summary;

        static MyCommands()
        {
            SetStartNode = new RoutedUICommand("SetStartNode", "SetStartNode", typeof(MyCommands));
            SetEndNode = new RoutedUICommand("SetEndNode", "SetEndNode", typeof(MyCommands));
            CleanLabyrinth = new RoutedUICommand("CleanLabyrinth", "CleanLabyrinth", typeof(MyCommands));
            ResetLabyrinth = new RoutedUICommand("ResetLabyrinth", "ResetLabyrinth", typeof(MyCommands));
            InvertLabyrinth = new RoutedUICommand("InvertLabyrinth", "InvertLabyrinth", typeof(MyCommands));
            StartAlghoritm = new RoutedUICommand("StartAlghoritm", "StartAlghoritm", typeof(MyCommands));


            StopAlghoritm = new RoutedUICommand("StopAlghoritm", "StopAlghoritm", typeof(MyCommands));
            SaveAlghoritm = new RoutedUICommand("SaveAlghoritm", "SaveAlghoritm", typeof(MyCommands));
            LoadAlghoritm = new RoutedUICommand("LoadAlghoritm", "LoadAlghoritm", typeof(MyCommands));
            Summary = new RoutedUICommand("Summary", "Summary", typeof(MyCommands));



        }



    }
}
