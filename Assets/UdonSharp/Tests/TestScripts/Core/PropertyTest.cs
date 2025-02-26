﻿
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace UdonSharp.Tests
{
    [AddComponentMenu("Udon Sharp/Tests/PropertyTest")]
    public class PropertyTest : UdonSharpBehaviour
    {
        [HideInInspector]
        public IntegrationTestSuite tester;

        public string MyStrProperty => "Test " + 1;
        
        public int MyIntProperty { get; set; }

        float backingFloat;
        public float MyFloatProperty { get => backingFloat; set => backingFloat = value; }

        int backingField;
        public int MyIntBackedProperty
        {
            get { return backingField; }
            set
            {
                MyIntProperty = value;
                backingField = value;
            }
        }

        public PropertyTest MyUserTypeProperty { get { return this; } }

        int accessCounter = 0;
        public int MyAccessCounter {  set { /*Debug.Log("Setting access counter to " + value); */accessCounter = value; } get { /*Debug.Log("Access counter: " + accessCounter);*/ return accessCounter++; } }

        public void ExecuteTests()
        {
            PropertyTest self = this;

            tester.TestAssertion("Local property getter 1", MyStrProperty == "Test 1");
            tester.TestAssertion("External property getter 1", self.MyStrProperty == "Test 1");

            tester.TestAssertion("Property initialized", MyIntProperty == 0);
            MyIntProperty = 1;
            tester.TestAssertion("Property set", MyIntProperty == 1);
            tester.TestAssertion("Property pre-increment", MyIntProperty++ == 1);
            tester.TestAssertion("Property post-increment", MyIntProperty == 2);

            self.MyIntProperty = 5;
            tester.TestAssertion("Property external set", MyIntProperty == 5);
            tester.TestAssertion("Property external get", self.MyIntProperty == 5);

            MyIntBackedProperty = 8;
            tester.TestAssertion("Property manual setter", MyIntProperty == 8 && MyIntBackedProperty == 8);

            PropertyTest[] selfArr = new PropertyTest[] { this };

            selfArr[0].MyIntProperty = 10;

            tester.TestAssertion("Array property access 1", selfArr[0].MyIntProperty == 10);
            tester.TestAssertion("Array property access 2", ++selfArr[0].MyIntProperty == 11);
            tester.TestAssertion("Array property access 3", selfArr[0].MyIntProperty++ == 11);
            tester.TestAssertion("Array property access 4", selfArr[0].MyIntProperty == 12);

            MyUserTypeProperty.MyIntBackedProperty = 2;
            tester.TestAssertion("User type maintained", MyIntProperty == 2);

            self = MyUserTypeProperty;
            MyUserTypeProperty.MyIntProperty = 4;

            tester.TestAssertion("User type maintained 2", MyIntProperty == 4);

            backingFloat = 5f;
            MyFloatProperty = (int)4;

            tester.TestAssertion("Implicit conversion type maintained", backingFloat.GetType() == typeof(float));

            tester.TestAssertion("Access counter 1", MyAccessCounter == 0);
            tester.TestAssertion("Access counter 2", MyAccessCounter == 1);

            tester.TestAssertion("Access counter intermediates", MyAccessCounter + MyAccessCounter == 5);
            tester.TestAssertion("Access counter external intermediates", self.MyAccessCounter + self.MyAccessCounter == 9);

            tester.TestAssertion("Access counter increment", ++MyAccessCounter == 7);
            tester.TestAssertion("Access counter increment 2", MyAccessCounter == 7);
            tester.TestAssertion("Access counter increment 3", MyAccessCounter++ == 8);
            tester.TestAssertion("Access counter increment 4", MyAccessCounter == 9);
            tester.TestAssertion("Access counter increment 5", MyAccessCounter == 10);

            //Debug.Log("Start tests");

            //int preIncrementResult = ++MyAccessCounter;
            //Debug.Log("Pre inc " + preIncrementResult);
            //Debug.Log("Pre inc access counter " + accessCounter);

            //int postIncrementResult = MyAccessCounter++;
            //Debug.Log("Post inc " + postIncrementResult);
            //Debug.Log("Post inc access counter " + accessCounter);

            //Debug.Log("In place addition");

            int additionResult = MyAccessCounter += 6;

            tester.TestAssertion("In place addition", accessCounter == 17 && additionResult == 17);
        }
    }
}
