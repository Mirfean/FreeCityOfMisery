using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlacingCompareSidesTestEdit
{
    GameObject testObject;

    List<PowerType> testListEmpty = new List<PowerType>();
    List<PowerType> testListXXX = new List<PowerType>();
    List<PowerType> testList01 = new List<PowerType>();
    List<PowerType> testList02 = new List<PowerType>();
    List<PowerType> testList03 = new List<PowerType>();

    [SetUp]
    public void init()
    {
        testObject = new GameObject();
        testObject.AddComponent<PlacingManager>();
    }

    [OneTimeSetUp]
    public void PrepareLists()
    {
        testListXXX.Add(PowerType.XXX);
        
        testList01.Add(PowerType.CHARM);
        testList01.Add(PowerType.CHARM);
        testList01.Add(PowerType.AGGRESSION);
        
        testList02.Add(PowerType.CHARM);
        testList02.Add(PowerType.AUTHORITY);
        
        testList03.Add(PowerType.LOGIC);

    }

    [OneTimeTearDown]
    public void ClearLists()
    {
        testListXXX.Clear();
        testList01.Clear();
        testList02.Clear();
        testList03.Clear();

    }

    [Test]
    public void ListkowyTest()
    {
        Assert.AreEqual(4, 2+2);
    }

    // A Test behaves as an ordinary method
    [Test]
    public void CompareSides_Empty_Empty()
    {
        var result = testObject.GetComponent<PlacingManager>().CompareSides(testListEmpty, testListEmpty);
        Assert.IsTrue(result.Item1);
        Assert.IsNull(result.Item2);
    }

    [Test]
    public void CompareSides_Empty_XXX()
    {
        var result = testObject.GetComponent<PlacingManager>().CompareSides(testListEmpty, testListXXX);
        Assert.IsTrue(result.Item1);
        Assert.IsNull(result.Item2);
    }

    [Test]
    public void CompareSides_XXX_Empty()
    {
        var result = testObject.GetComponent<PlacingManager>().CompareSides(testListXXX, testListEmpty);
        Assert.IsFalse(result.Item1);
        Assert.IsNull(result.Item2);
    }

    [Test]
    public void CompareSides_CCF_XXX()
    {
        var result = testObject.GetComponent<PlacingManager>().CompareSides(testList01, testListXXX);
        Assert.IsTrue(result.Item1);
        Assert.IsNull(result.Item2);
    }

    public void CompareSides_XXX_CCF()
    {
        var result = testObject.GetComponent<PlacingManager>().CompareSides(testListXXX, testList01);
        Assert.IsFalse(result.Item1);
        Assert.IsNull(result.Item2);
    }

    [Test]
    public void CompareSides_CCF_CCF()
    {
        var result = testObject.GetComponent<PlacingManager>().CompareSides(testList01, testList01);

        Assert.IsTrue(result.Item1);
        Assert.AreEqual(4, result.Item2.Count(x => x.Equals(PowerType.CHARM)));
        Assert.AreEqual(2, result.Item2.Count(x => x.Equals(PowerType.AGGRESSION)));
    }

    [Test]
    public void CompareSides_CA_CA()
    {
        var result = testObject.GetComponent<PlacingManager>().CompareSides(testList02, testList02);

        Assert.AreEqual(2, result.Item2.Count(x => x.Equals(PowerType.CHARM)));
        Assert.AreEqual(2, result.Item2.Count(x => x.Equals(PowerType.AUTHORITY)));
    }

    [Test]
    public void CompareSides_CCF_CA()
    {
        var result = testObject.GetComponent<PlacingManager>().CompareSides(testList01, testList02);

        Assert.IsTrue(result.Item1);
        Assert.AreEqual(3, result.Item2.Count(x => x.Equals(PowerType.CHARM)));
    }

    [Test]
    public void CompareSides_CA_L()
    {
        var result = testObject.GetComponent<PlacingManager>().CompareSides(testList02, testList03);
        Assert.IsFalse(result.Item1);
        Assert.IsNull(result.Item2);
    }

    [UnityTest]
    public IEnumerator PlacingTestEditWithEnumeratorPasses()
    {
        yield return null;
    }
}
