using PartituraCreator;
using PartituraCreator.Model;
using System.ComponentModel;
using System.Numerics;
using Vector3 = PartituraCreator.Model.Vector3;

class Program
{
    private Service _Service;

    public Program()
    {
        _Service = new Service();
    }

    private Guid AddPlayer(string[] parameter)
    {
        if (parameter.Length == 3)
        {
            try
            {
                bool leftHand = Convert.ToBoolean(parameter[0]);
                return _Service.AddPlayer(leftHand, parameter[1], parameter[2]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Guid.Empty;
            }
        }
        else
        {
            Console.WriteLine("Invalid number of parameter");
            return Guid.Empty;
        }
    }

    private void GetPlayer(string[] parameter)
    {
        if (parameter.Length == 1)
        {
            try
            {
                Guid id = Guid.Parse(parameter[0]);
                var player = _Service.GetPlayer(id);
                if (player == null)
                {
                    Console.WriteLine("Not player found");
                    return;
                }

                Console.WriteLine($"Player {player.Id}");
                Console.WriteLine($"Player use leftHand {player.LeftHand}");

                foreach (var obstacle in player.Obstacles)
                {
                    Console.WriteLine($"\t Obstacle {obstacle.Id}");
                    Console.WriteLine($"\t\t x: {obstacle.Position.x} y: {obstacle.Position.y} z: {obstacle.Position.z}");
                    Console.WriteLine($"\t\t Durazione {obstacle.Duration}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Invalid number of parameter");
        }
    }

    private void GetPlayers()
    {
        var players = _Service.GetPlayers();
        if (players == null || players.Count == 0)
        {
            Console.WriteLine("Not player found");
            return;
        }

        foreach (var player in players)
        {
            Console.WriteLine($"Player {player.Id}");
            Console.WriteLine($"Player use leftHand {player.LeftHand}");

            foreach (var obstacle in player.Obstacles)
            {
                Console.WriteLine($"\t Obstacle {obstacle.Id}");
                Console.WriteLine($"\t\t x: {obstacle.Position.x} y: {obstacle.Position.y} z: {obstacle.Position.z}");
                Console.WriteLine($"\t\t Durazione {obstacle.Duration}");
            }
        }
    }

    private void SetPlayer(string[] parameter)
    {
        if (parameter.Length == 4)
        {
            try
            {
                Guid id = Guid.Parse(parameter[0]);
                bool leftHand = Convert.ToBoolean(parameter[1]);
                var result = _Service.SetPlayer(id, leftHand, parameter[2], parameter[3]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Invalid number of parameter");
        }
    }

    private void DeletePlayer(string[] parameter)
    {
        if (parameter.Length == 1)
        {
            try
            {
                Guid id = Guid.Parse(parameter[0]);
                var result = _Service.DeletePlayer(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Invalid number of parameter");
        }
    }

    private void AddObstacle(string[] parameter)
    {
        if (parameter.Length == 5)
        {
            try
            {
                Guid playerId = Guid.Parse(parameter[0]);
                int duration = Convert.ToInt32(parameter[4]);
                float x = float.Parse(parameter[1]);
                float y = float.Parse(parameter[2]);
                float z = float.Parse(parameter[3]);
                Vector3 position = new Vector3(x, y, z);

                var result = _Service.AddObstacle(playerId, position, duration);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Invalid number of parameter");
        }
    }

    private void GetObstacle(string[] parameter)
    {
        if (parameter.Length == 4)
        {
            try
            {
                Guid obstacleId = Guid.Parse(parameter[0]);
                var obstacle = _Service.GetObstacle(obstacleId);
                if (obstacle == null)
                {
                    Console.WriteLine("Not obstacle found found");
                    return;
                }

                Console.WriteLine($"\t Obstacle {obstacle.Id}");
                Console.WriteLine($"\t\t x: {obstacle.Position.x} y: {obstacle.Position.y} z: {obstacle.Position.z}");
                Console.WriteLine($"\t\t Durazione {obstacle.Duration}");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Invalid number of parameter");
        }
    }

    private void SetObstacle(string[] parameter)
    {
        if (parameter.Length == 4)
        {
            try
            {
                Guid obstacleId = Guid.Parse(parameter[0]);
                int duration = Convert.ToInt32(parameter[4]);
                float x = float.Parse(parameter[1]);
                float y = float.Parse(parameter[2]);
                float z = float.Parse(parameter[3]) + 20;
                Vector3 position = new Vector3(x, y, z);

                var result = _Service.SetObstacle(obstacleId, position, duration);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Invalid number of parameter");
        }
    }

    private void DeleteObstacle(string[] parameter)
    {
        if (parameter.Length == 1)
        {
            try
            {
                Guid id = Guid.Parse(parameter[0]);
                var result = _Service.DeleteObstacle(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Invalid number of parameter");
        }
    }

    private void Print(string[] parameter)
    {
        if (parameter.Length == 1)
        {
            try
            {
                var result = _Service.PrintAsync(parameter[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Invalid number of parameter");
        }
    }

    private void Read(string[] parameter)
    {
        if (parameter.Length == 1)
        {
            try
            {
                var result = _Service.ReadAsync(parameter[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Invalid number of parameter");
        }
    }

    private void PrintListOfCommand()
    {
        var command = GetCommand();
        Console.WriteLine("\n List of command:");
        foreach (var key in command.Keys)
            Console.WriteLine($"\t {key} - {command[key]}");
    }

    private void SetIpAddress(string[] parameter)
    {
        if (parameter.Length == 1)
        {
            try
            {
                var result = _Service.AddIpAddress(parameter[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Invalid number of parameter");
        }
    }

    private void Add()
    {
        Console.WriteLine("Enter in ADD modde");
        Console.WriteLine("\t Type 'new' to add new player");
        Console.WriteLine("\t Type 'exit' to exit from ADD mode");

        Guid playerId = Guid.Empty;

        bool endApp = false;
        while (!endApp)
        {
            string cmd = Console.ReadLine();
            if (cmd == "exit") return;

            if(cmd == "new")
            {
                Console.WriteLine("New player. Insert {bool: lefthand} {string: inPort} {string: outPort}");
                string[] input = Console.ReadLine().Split(" ");
                var id = AddPlayer(input);
                if (id != Guid.Empty)
                {
                    playerId = id;
                    Console.WriteLine("Add obstacles {float: x} {float: y} {float: z} {int: duration}");
                }
            }
            else if(playerId != Guid.Empty)
            {
                List<string> input = cmd.Split(" ").ToList();
                 input.Insert(0, playerId.ToString());

                AddObstacle(input.ToArray());
            }
            else
            {
                Console.WriteLine("Invalid command");
            }
        }
    }

    private void Random()
    {
        Console.WriteLine("Enter in RANDOM modde");
        Console.WriteLine("\t Type 'new to add new player");
        Console.WriteLine("\t Type 'exit' to exit from RANDOM mode");

        Random rnd = new Random();

        try
        {       
            bool endApp = false;
            while (!endApp)
            {
                string cmd = Console.ReadLine();
                if (cmd == "exit") 
                    return;
                else if (cmd == "new")
                {
                    Console.WriteLine("New player. Insert {bool: lefthand} {string: inPort} {string: outPort}");
                    string[] input = Console.ReadLine().Split(" ");
                    var playerId = AddPlayer(input);

                    Console.WriteLine("Insert number of obstacle ");
                    input = Console.ReadLine().Split(" ");
                    if (input.Length > 1)
                        throw new Exception("Invalid number of command");
                    int number = Convert.ToInt32(input[0]);

                    int distance = 0;
                    for(int i=0; i<number; i++)
                    {
                        Vector3 pos = new Vector3(rnd.Next(10), rnd.Next(10), distance);
                        _Service.AddObstacle(playerId, pos, rnd.Next(1,20));
                        distance += rnd.Next(3, 15);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid command");
                }
            }
        }
        catch
        {
            Console.WriteLine("Invalid command");
        }
    }


    private IDictionary<string, string> GetCommand()
    {
        Dictionary<string, string> commands = new();
        commands.Add("read {string: path}", "Read a configuration file for Hallway in JSON format");
        commands.Add("print {string: path}", "Create a configuration file for Hallway in JSON format");
        commands.Add("ip {string: ip}", "Set Ip address to forward messages");        
        commands.Add("add", "Add player and his obstacles");
        commands.Add("random", "Add player and his obstacles in random way");
        commands.Add("add-player {bool: lefthand} {string: inPort} {string: outPort}", "Add new player");
        commands.Add("get-player {guid: id}", "Get player");
        commands.Add("get-players", "Get all players");
        commands.Add("edit-player {bool: lefthand} {string: inPort} {string: outPort}", "Edit a player");
        commands.Add("delete-player {guid: id}", "Delete a player");
        commands.Add("add-obstacle {guid: playerId} {float: x} {float: y} {float: z} {int: duration}", "Add new obstacle for player");
        commands.Add("get-obstacle {guid: id}", "Get an obstacle for player");
        commands.Add("edit-obstacle {guid: id} {float: x} {float: y} {float: z} {int: duration}", "Add new obstacle for player");
        commands.Add("delete-obstacle {guid: id}", "Delete an obstacle for player");
        commands.Add("help", "Get commands");

        return commands;
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Crea il tuo file di configurazione per Hallway\r");
        Console.WriteLine("------------------------\n");

        Program program = new Program();

        program.PrintListOfCommand();

        bool endApp = false;
        while (!endApp)
        {
            string[] input = Console.ReadLine().Split(" ");
            string cmd = input[0];
            switch (cmd)
            {
                case "add-player":
                    program.AddPlayer(input.Skip(1).Take(input.Length).ToArray());
                    break;

                case "get-player":
                    program.GetPlayer(input.Skip(1).Take(input.Length).ToArray());
                    break;

                case "get-players":
                    program.GetPlayers();
                    break;

                case "edit-player":
                    program.SetPlayer(input.Skip(1).Take(input.Length).ToArray());
                    break;

                case "delete-player":
                    program.DeletePlayer(input.Skip(1).Take(input.Length).ToArray());
                    break;

                case "add-obstacle":
                    program.AddObstacle(input.Skip(1).Take(input.Length).ToArray());
                    break;

                case "get-obstacle":
                    program.GetObstacle(input.Skip(1).Take(input.Length).ToArray());
                    break;

                case "edit-obstacle":
                    program.SetObstacle(input.Skip(1).Take(input.Length).ToArray());
                    break;

                case "delete-obstacle":
                    program.DeleteObstacle(input.Skip(1).Take(input.Length).ToArray());
                    break;

                case "add":
                    program.Add();
                    break;

                case "random":
                    program.Random();
                    break;

                case "print":
                    program.Print(input.Skip(1).Take(input.Length).ToArray());
                    break;

                case "read":
                    program.Read(input.Skip(1).Take(input.Length).ToArray());
                    break;

                case "help":
                    program.PrintListOfCommand();
                    break;

                case "ip":
                    program.SetIpAddress(input.Skip(1).Take(input.Length).ToArray());
                    break;

                default:
                    Console.WriteLine("Not valid command found");
                    break;
            }
        }
    }
}