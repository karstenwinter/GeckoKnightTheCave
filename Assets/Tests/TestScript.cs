using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class TestScript
{
    // A Test behaves as an ordinary method
    //[Test]
    //public void NewTestScriptSimplePasses()
    //{
    //}

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses()
    {
        SceneManager.LoadScene("SampleScene");
        Thread.Sleep(2000);
        //Assert.That(PlayerController.instance != null);
        Assert.That(InputCanvas.instance != null);

        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        while (true)
            yield return null;
    }
}