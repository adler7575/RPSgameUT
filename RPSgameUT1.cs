using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using RPSgame.Messages;
using RPSgame.Models;
using RSPgame.Controllers;
using System;

namespace RPSgameUT
{
    [TestClass]
    public class RPSgameUT1
    {
        /// <summary>
        /// Create two different games duing the same run
        /// </summary>
        [TestMethod]
        public void CreateGame1()
        {
            Player P = new Player("Anna");
            GameController GameCo = new GameController();
            Object Obj = GameCo.NewGame(P);
            Console.WriteLine(Obj.GetType().Name);
            // Check the return type           
            Assert.IsTrue(Obj.GetType().Name.CompareTo("OkObjectResult") == 0);

            IdMessage Idm1  =(IdMessage)(((OkObjectResult)Obj).Value);
                        

            // Create a new game
            P = new Player("Emelie");
            Obj = GameCo.NewGame(P);
            IdMessage Idm2 = ((IdMessage)((OkObjectResult)Obj).Value);

            // Check that a rew game has been started
            Assert.AreNotEqual(Idm1.Id, Idm2.Id);
            Assert.IsTrue(GameController.gGameList.Count == 2);
        }


        /// <summary>
        /// Create a game and add a player with two different player names
        /// </summary>
        [TestMethod]
        public void CreateGame2()
        {
            Player P = new Player("Anna");
            GameController GameCo = new GameController();
            Object Obj = GameCo.NewGame(P);
            Assert.IsTrue(Obj.GetType().Name.CompareTo("OkObjectResult") == 0);

            IdMessage Idm1 = ((IdMessage)((OkObjectResult)Obj).Value);

            P = new Player("Emelie");
            Obj = GameCo.Joingame(Idm1.Id, P);

            Assert.IsTrue(Obj.GetType().Name.CompareTo("RPSGameDTO") == 0);

            RPSGameDTO gameDTO = (RPSGameDTO)Obj;

            // Check that a rew game has been started
            Assert.AreEqual(gameDTO.Id, Idm1.Id);
        }

        /// <summary>
        /// Create a game with 2 players with the same name
        /// </summary>
        [TestMethod]
        public void CreateGame3()
        {
            Player P = new Player("Anna");
            GameController GameCo = new GameController();
            Object Obj = GameCo.NewGame(P);

            IdMessage Idm1 = (IdMessage)(((OkObjectResult)Obj).Value);

            P = new Player("Anna");

            Obj = GameCo.Joingame(Idm1.Id, P);
            Console.WriteLine(Obj.GetType().Name);
            Assert.IsTrue(Obj.GetType().Name.CompareTo("BadRequestObjectResult") == 0);
        }

        /// <summary>
        /// Test a bad empty player name
        /// </summary>
        [TestMethod]
        public void CreateGame4()
        {
            Player P = new Player();
            GameController GameCo = new GameController();
            Object Obj = GameCo.NewGame(P);
            Console.WriteLine(Obj.GetType().Name);
            // Check the return type           
            Assert.IsFalse(Obj.GetType().Name.CompareTo("BadReuestObjectResult") == 0);
        }

        /// <summary>
        /// Test a game with a bad move
        /// </summary>
        [TestMethod]
        public void CreateGame5()
        {
            Player P = new Player("Anna");
            GameController GameCo = new GameController();
            Object Obj = GameCo.NewGame(P);
            IdMessage Idm1 = ((IdMessage)((OkObjectResult)Obj).Value);

            GameCo.Joingame(Idm1.Id, new Player("Emelie"));            
            bool testok = false;
            try 
            {
                P.GameMove = "paperr";
                GameCo.Move(Idm1.Id, P);
            }
            catch (Exception)
            {
                testok = true;
            }
            Assert.IsTrue(testok);
        }

        /// <summary>
        /// Test a game with a two equal moves
        /// </summary>
        [TestMethod]
        public void CreateGame6()
        {
            GameController GameCo = new GameController();            

            Player P1 = new Player("Anna");
            Player P2 = new Player("Emelie");
            Object Obj = GameCo.NewGame(P1);
            IdMessage Idm1 = ((IdMessage)((OkObjectResult)Obj).Value);
            
            GameCo.Joingame(Idm1.Id, P2);
            P1.GameMove = "rock";
            GameCo.Move(Idm1.Id, P1);
            P2.GameMove = "rock";
            GameCo.Move(Idm1.Id, P2);
            RPSGameDTO gameDTO = (RPSGameDTO)GameCo.GetStatus(Idm1.Id);
            Console.WriteLine(gameDTO.Winner);
            Assert.IsTrue(gameDTO.Winner == "Draw");
        }
        /// <summary>
        /// Test a game where rock wins over scissors
        /// </summary>
        [TestMethod]
        public void CreateGame7()
        {
            GameController GameCo = new GameController();

            Player P1 = new Player("Anna");
            Player P2 = new Player("Emelie");
            Object Obj = GameCo.NewGame(P1);
            IdMessage Idm1 = ((IdMessage)((OkObjectResult)Obj).Value);

            GameCo.Joingame(Idm1.Id, P2);
            P1.GameMove = "scissors";
            GameCo.Move(Idm1.Id, P1);
            P2.GameMove = "rock";
            GameCo.Move(Idm1.Id, P2);
            RPSGameDTO gameDTO = (RPSGameDTO)GameCo.GetStatus(Idm1.Id);
            Console.WriteLine(gameDTO.Winner);
            Assert.IsTrue(gameDTO.Winner == P2.PlayerName);
        }

        /// <summary>
        /// Test a game where paper wins over rock
        /// </summary>
        [TestMethod]
        public void CreateGame8()
        {
            GameController GameCo = new GameController();

            Player P1 = new Player("Anna");
            Player P2 = new Player("Emelie");
            Object Obj = GameCo.NewGame(P1);
            IdMessage Idm1 = ((IdMessage)((OkObjectResult)Obj).Value);
            GameCo.Joingame(Idm1.Id, P2);

            P1.GameMove = "paper";
            GameCo.Move(Idm1.Id, P1);
            P2.GameMove = "rock";
            GameCo.Move(Idm1.Id, P2);
            RPSGameDTO gameDTO = (RPSGameDTO)GameCo.GetStatus(Idm1.Id);
            Console.WriteLine(gameDTO.Winner);
            Assert.IsTrue(gameDTO.Winner == P1.PlayerName);
        }

        /// <summary>
        /// Test a game where scissors wins over paper
        /// </summary>
        [TestMethod]
        public void CreateGame9()
        {
            GameController GameCo = new GameController();

            Player P1 = new Player("Anna");
            Player P2 = new Player("Emelie");
            Object Obj = GameCo.NewGame(P1);
            IdMessage Idm1 = ((IdMessage)((OkObjectResult)Obj).Value);
            GameCo.Joingame(Idm1.Id, P2);

            P1.GameMove = "scissors";
            GameCo.Move(Idm1.Id, P1);
            P2.GameMove = "paper";
            GameCo.Move(Idm1.Id, P2);
            RPSGameDTO gameDTO = (RPSGameDTO)GameCo.GetStatus(Idm1.Id);
            Console.WriteLine(gameDTO.Winner);
            Assert.IsTrue(gameDTO.Winner == P1.PlayerName);
        }


    }
}
