using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


namespace Assets.Scripts.Tests.EditMode
{
    public class PlacingGetBestCellTestEdit
    {
        GameObject testObject, goPieceEmpty, goPiece02, goPiece03, goPiece04;

        GamePiece[] cardsFull4;
        
        HashSet<PositionGrid> positionGrids;
        PositionGrid pgEmpty;
        PositionGrid pg02;
        PositionGrid pg03;

        [OneTimeSetUp]
        public void init()
        {
            testObject = new GameObject();
            testObject.AddComponent<PlacingManager>();

            goPieceEmpty = new GameObject();
            goPieceEmpty.AddComponent<GamePiece>();
            goPieceEmpty.GetComponent<GamePiece>().PrepareNewLists();

            goPiece02 = new GameObject();
            goPiece02.AddComponent<GamePiece>();
            goPiece02.GetComponent<GamePiece>().PrepareNewLists();

            goPiece03 = new GameObject();
            goPiece03.AddComponent<GamePiece>();
            goPiece03.GetComponent<GamePiece>().PrepareNewLists();

            goPiece04 = new GameObject();
            goPiece04.AddComponent<GamePiece>();
            goPiece04.GetComponent<GamePiece>().PrepareNewLists();

            pgEmpty = new PositionGrid(0, 0);
            pg02 = new PositionGrid(0, 2);
            pg03 = new PositionGrid(0, 3);

            goPieceEmpty.name = "GP00";

            goPiece02.GetComponent<GamePiece>().side0.Add(PowerType.CHARM);
            goPiece02.GetComponent<GamePiece>().side2.Add(PowerType.CHARM);
            goPiece02.name = "GP02";

            goPiece03.GetComponent<GamePiece>().side0.Add(PowerType.CHARM);
            goPiece03.GetComponent<GamePiece>().side1.Add(PowerType.AUTHORITY);
            goPiece03.name = "GP03";

            goPiece04.GetComponent<GamePiece>().side1.Add(PowerType.CHARM);
            goPiece04.GetComponent<GamePiece>().side1.Add(PowerType.CHARM);
            goPiece04.GetComponent<GamePiece>().side2.Add(PowerType.AUTHORITY);
            goPiece04.name = "GP04";

            cardsFull4 = new GamePiece[4];
            cardsFull4[0] = goPieceEmpty.GetComponent<GamePiece>();
            cardsFull4[1] = goPiece02.GetComponent<GamePiece>();
            cardsFull4[2] = goPiece03.GetComponent<GamePiece>();
            cardsFull4[3] = goPiece04.GetComponent<GamePiece>();

            pg02.side2Powers.Add(PowerType.CHARM);
            pg02.side2Powers.Add(PowerType.CHARM);

            pg03.side1Powers.Add(PowerType.AGGRESSION);
            pg03.side0Powers.Add(PowerType.CHARM);

            positionGrids = new HashSet<PositionGrid>();
            positionGrids.Add(pgEmpty);
            positionGrids.Add(pg02);
            positionGrids.Add(pg03);
        }

        public (bool, int, int, PositionGrid, Dictionary<PowerType, int>) GetBestCell(GamePiece[] cards, HashSet<PositionGrid> positionGrids)
        {
            int rotation = 0;
            int bestScore = 0;
            PositionGrid best = new PositionGrid(-1, -1);
            Dictionary<PowerType, int> correlations = new Dictionary<PowerType, int>();

            void CheckAndCompare(int toRotate, PositionGrid positionGrid, GamePiece gamePiece)
            {
                GamePiece tempGP = gamePiece;

                for (int i = 0; i < toRotate; i++) tempGP.Rotate201();

                var result = testObject.GetComponent<PlacingManager>().CheckAvailable(positionGrid, tempGP);
                if (result.Item1)
                {
                    if (bestScore == 0 && result.Item2 == 0)
                    {
                        Debug.Log($"Setting on {positionGrid.x} {positionGrid.y} gp {gamePiece.name} with score {result.Item2} and rotation {toRotate}");
                        bestScore = result.Item2;
                        rotation = toRotate;
                        best = positionGrid;
                        correlations = result.Item3;
                    }
                    if (result.Item2 > bestScore)
                    {
                        Debug.Log($"Setting on {positionGrid.x} {positionGrid.y} gp {gamePiece.name} with score {result.Item2} and rotation {toRotate}");
                        bestScore = result.Item2;
                        rotation = toRotate;
                        best = positionGrid;
                        correlations = result.Item3;
                    }
                }
            }
            if (cards.Length == 0) return (false, 0, 0, new PositionGrid(), null);

            foreach (GamePiece card in cards)
            {
                foreach (PositionGrid positionGrid in positionGrids)
                {
                    CheckAndCompare(0, positionGrid, card);
                    CheckAndCompare(1, positionGrid, card);
                    CheckAndCompare(2, positionGrid, card);
                }
            }

            if (best.x >= 0 && best.y >= 0)
            {
                return (true, bestScore, rotation, best, correlations);
            }

            return (false, 0, 0, new PositionGrid(), null);
        }

        [Test]
        public void finalTest()
        {
            var result = GetBestCell(cardsFull4, positionGrids);

            Assert.IsTrue(result.Item1, "Best piece was found");
            Assert.AreEqual(4, result.Item2, "score " + result.Item2);
            Assert.AreEqual(1, result.Item3, "Rotation " + result.Item3);
            foreach(KeyValuePair<PowerType, int> corr in result.Item5)
            {
                Debug.Log($"Key {corr.Key} {corr.Value}");
            }
        }
    }
}
