using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlacingCheckAvailableTestEdit
{
    GameObject testObject, goPieceEmpty, goPiece02, goPiece03;
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

        pgEmpty = new PositionGrid(0,0);
        pg02 = new PositionGrid(0,0);
        pg03 = new PositionGrid(0,0);

        goPiece02.GetComponent<GamePiece>().side0.Add(PowerType.CHARM);
        goPiece02.GetComponent<GamePiece>().side2.Add(PowerType.CHARM);

        goPiece03.GetComponent<GamePiece>().side0.Add(PowerType.CHARM);
        goPiece03.GetComponent<GamePiece>().side1.Add(PowerType.AUTHORITY);

        pg02.side2Powers.Add(PowerType.CHARM);
        pg02.side2Powers.Add(PowerType.CHARM);

        pg03.side1Powers.Add(PowerType.AGGRESSION);
        pg03.side0Powers.Add(PowerType.CHARM);


    }

    [Test]
    public void Empty_Empty()
    {
        var result = testObject.GetComponent<PlacingManager>()
            .CheckAvailable(pgEmpty, goPieceEmpty.GetComponent<GamePiece>());
        Assert.True(result.Item1);
        Assert.AreEqual(0, result.Item2);
    }

    [Test]
    public void CC2()
    {
        var result = testObject.GetComponent<PlacingManager>()
            .CheckAvailable(pg02, goPiece02.GetComponent<GamePiece>());

        Assert.True(result.Item1);
        Assert.AreEqual(3, result.Item2);
        Assert.AreEqual(3, result.Item3[PowerType.CHARM]);
    }

    [Test]
    public void CC_Empty()
    {
        var result = testObject.GetComponent<PlacingManager>()
            .CheckAvailable(pg02, goPieceEmpty.GetComponent<GamePiece>());

        Assert.True(result.Item1);
    }

    [Test]
    public void F1C0_C0A1()
    {
        var result = testObject.GetComponent<PlacingManager>()
            .CheckAvailable(pg03, goPiece03.GetComponent<GamePiece>());

        Assert.False(result.Item1);
        Assert.AreEqual(2, result.Item2);
        Assert.AreEqual(null, result.Item3);
    }



}
