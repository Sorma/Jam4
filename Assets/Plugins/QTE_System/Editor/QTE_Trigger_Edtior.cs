/* CUstom Editor for QTE_Trigger.cs*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(QTE_Trigger))]
public class QTE_Trigger_Edtior : Editor
{

    bool MainSettings = true;
    bool InputOptions = true;
    bool TimerOptions = true;
    bool DisplayOptions = true;


    SerializedProperty QTEtype;

    string[] options;

    public override void OnInspectorGUI()
    {

        serializedObject.Update();

        QTE_Trigger myTarget = (QTE_Trigger)target;
        myTarget.ListOfInputs = GetInputAxis();

        options = myTarget.myNewDirections;

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.Space();
        QTEtype = serializedObject.FindProperty("QTEtype");

        MainSettings = EditorGUILayout.Foldout(MainSettings, "Main Settings");
        if (MainSettings)
        {
            EditorGUILayout.BeginVertical("Box");

            myTarget.Canvas = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Overide Canvas:", "The Canvas To Use"), myTarget.Canvas, typeof(GameObject), true);

            EditorGUILayout.PropertyField(QTEtype, new GUIContent("QTE Type:", "'Collider' if attached to a gameobject with a Collider, 'Manual' otherwise"), true);

            if (myTarget.GetComponent<Collider>() != null && myTarget.GetComponent<Collider>().isTrigger)
            {

                EditorGUILayout.LabelField("Mode:", "Using Collider");
                myTarget.Repeatable = EditorGUILayout.Toggle(new GUIContent("Repeatable", "If true, the QTE will always fire every time the player enters the collider, if false, only fires the first time."), myTarget.Repeatable);
                serializedObject.ApplyModifiedProperties();
            }
            else
            {
                EditorGUILayout.LabelField("Mode:", "Manual");
            }
        }
        //myTarget.EnableTouch = EditorGUILayout.Toggle (new GUIContent ("Enable Touch Input", ""), myTarget.EnableTouch);

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
        InputOptions = EditorGUILayout.Foldout(InputOptions, "Input Options");

        if (InputOptions)
        {
            EditorGUILayout.BeginVertical("Box");
            if (myTarget.EnableTouch)
            {/*
				myTarget.EnableButtonFail = EditorGUILayout.Toggle (new GUIContent ("Allow Touch Failure", "If true, QTE fails if the player does a gesture that is not what the QTE is looking for"), myTarget.EnableButtonFail);

				//EditorGUILayout.PropertyField (EditorEnableButtonFail, new GUIContent ("Allow Wrong Button Failure", "If true, when player Presses a button that's not the correct one, the QTE will fail. If false, the QTE persists until succeeded or times out."), true);
				myTarget.TouchHit = EditorGUILayout.Toggle (new GUIContent ("Button Hit", "Off -> standard keyboard input is used On -> the script will use Unity's buttons/Axis defined in the Input manager instead."), myTarget.TouchHit);

				if(myTarget.TouchType == QTE_Trigger.TouchTypes.Swipe || myTarget.TouchType2 == QTE_Trigger.TouchTypes.Swipe || myTarget.TouchType3 == QTE_Trigger.TouchTypes.Swipe || myTarget.TouchType4 == QTE_Trigger.TouchTypes.Swipe){
					myTarget.minSwipeDist = EditorGUILayout.FloatField ("Swipe Distance", myTarget.minSwipeDist);
					myTarget.maxSwipeTime = EditorGUILayout.FloatField ("Swipe Time", myTarget.maxSwipeTime);
				}
				EditorGUILayout.LabelField ("Button 1", EditorStyles.boldLabel);
				myTarget.TouchType = (QTE_Trigger.TouchTypes)EditorGUILayout.EnumPopup ("\tTouch Type", myTarget.TouchType);

				if (myTarget.TouchType == QTE_Trigger.TouchTypes.Hold) {
					myTarget.TouchHoldTime = EditorGUILayout.FloatField ("\tHold Length", myTarget.TouchHoldTime);
				}

				if(myTarget.TouchType == QTE_Trigger.TouchTypes.Swipe){
					myTarget.RandomSwipeDirections[0] = EditorGUILayout.Toggle (new GUIContent ("\tRandom Swipe Direction", "Off -> Input is a button press On -> Input is an Axis"), myTarget.RandomSwipeDirections[0]);
					if(!myTarget.RandomSwipeDirections[0]){
						myTarget.SwipeDirectionChoices[0] = EditorGUILayout.Popup("\tSwipe Direction",myTarget.SwipeDirectionChoices[0], myTarget.SwipeDirections);	
					}
				}

				if (QTEtype.enumValueIndex == 1 || QTEtype.enumValueIndex == 2 || QTEtype.enumValueIndex == 3) {
					EditorGUILayout.LabelField ("Button 2", EditorStyles.boldLabel);
					myTarget.TouchType2 = (QTE_Trigger.TouchTypes)EditorGUILayout.EnumPopup ("\tTouch Type", myTarget.TouchType2);

					if (myTarget.TouchType2 == QTE_Trigger.TouchTypes.Hold) {
						myTarget.TouchHoldTime2 = EditorGUILayout.FloatField ("\tHold Length", myTarget.TouchHoldTime2);
					}

					if(myTarget.TouchType2 == QTE_Trigger.TouchTypes.Swipe){
						myTarget.RandomSwipeDirections[1] = EditorGUILayout.Toggle (new GUIContent ("\tRandom Swipe Direction", "Off -> Input is a button press On -> Input is an Axis"), myTarget.RandomSwipeDirections[1]);
						if(!myTarget.RandomSwipeDirections[1]){
							myTarget.SwipeDirectionChoices[1] = EditorGUILayout.Popup("\tSwipe Direction",myTarget.SwipeDirectionChoices[1], myTarget.SwipeDirections);	
						}
					}
				}
				if (QTEtype.enumValueIndex == 2 || QTEtype.enumValueIndex == 3) {
					EditorGUILayout.LabelField ("Button 3", EditorStyles.boldLabel);
					myTarget.TouchType3 = (QTE_Trigger.TouchTypes)EditorGUILayout.EnumPopup ("\tTouch Type", myTarget.TouchType3);

					if (myTarget.TouchType3 == QTE_Trigger.TouchTypes.Hold) {
						myTarget.TouchHoldTime3 = EditorGUILayout.FloatField ("\tHold Length", myTarget.TouchHoldTime3);
					}

					if(myTarget.TouchType3 == QTE_Trigger.TouchTypes.Swipe){
						myTarget.RandomSwipeDirections[2] = EditorGUILayout.Toggle (new GUIContent ("\tRandom Swipe Direction", "Off -> Input is a button press On -> Input is an Axis"), myTarget.RandomSwipeDirections[2]);
						if(!myTarget.RandomSwipeDirections[2]){
							myTarget.SwipeDirectionChoices[2] = EditorGUILayout.Popup("\tSwipe Direction",myTarget.SwipeDirectionChoices[2], myTarget.SwipeDirections);	
						}				 
					}
				}
				if (QTEtype.enumValueIndex == 3) {
					EditorGUILayout.LabelField ("Button 4", EditorStyles.boldLabel);
					myTarget.TouchType4 = (QTE_Trigger.TouchTypes)EditorGUILayout.EnumPopup ("\tTouch Type", myTarget.TouchType4);

					if (myTarget.TouchType4 == QTE_Trigger.TouchTypes.Hold) {
						myTarget.TouchHoldTime4 = EditorGUILayout.FloatField ("\tHold Length", myTarget.TouchHoldTime4);
					}

					if(myTarget.TouchType4 == QTE_Trigger.TouchTypes.Swipe){
						myTarget.RandomSwipeDirections[3] = EditorGUILayout.Toggle (new GUIContent ("\tRandom Swipe Direction", "Off -> Input is a button press On -> Input is an Axis"), myTarget.RandomSwipeDirections[3]);
						if(!myTarget.RandomSwipeDirections[3]){
							myTarget.SwipeDirectionChoices[3] = EditorGUILayout.Popup("\tSwipe Direction",myTarget.SwipeDirectionChoices[3], myTarget.SwipeDirections);	
						}				 
					}
				}
				//SerializedProperty Touches = serializedObject.FindProperty ("Touches");
				//EditorGUILayout.PropertyField(Touches,new GUIContent("", ""),true);

			*/
            }
            else
            {
                if (QTEtype.enumValueIndex == 4)
                {
                    SerializedProperty Editortolerance = serializedObject.FindProperty("tolerance");
                    EditorGUILayout.PropertyField(Editortolerance, new GUIContent("Mashing Tolerance", "There is a float value that starts with 0, every time the player presses the button it goes up by 0.2, but every frame the value of tolerance is subtracted from it, hence the 'fighting' struggle. Player succeeds when value reaches 1.Increase tolerance to make the amount of button mashing harder to do, less to make it more easy."), true);
                }

                SerializedProperty EditorEnableButtonFail = serializedObject.FindProperty("EnableButtonFail");
                EditorGUILayout.PropertyField(EditorEnableButtonFail, new GUIContent("Allow Wrong Button Failure", "If true, when player Presses a button that's not the correct one, the QTE will fail. If false, the QTE persists until succeeded or times out."), true);

                myTarget.UseInputs = EditorGUILayout.Toggle(new GUIContent("Use Unity Inputs", "Off -> standard keyboard input is used On -> the script will use Unity's buttons/Axis defined in the Input manager instead."), myTarget.UseInputs);

                serializedObject.ApplyModifiedProperties();
                EditorGUILayout.LabelField("Button 1", EditorStyles.boldLabel);

                serializedObject.Update();
                myTarget.UseRandomButtons[0] = EditorGUILayout.Toggle(new GUIContent("\tRandomize", "Check true if you want to use an Randomly Generated button"), myTarget.UseRandomButtons[0]);
                
                if (!myTarget.UseRandomButtons[0])
                {
                    if (myTarget.UseInputs)
                    {
                        //EditorKeyPress = serializedObject.FindProperty ("Button1KeyPress");
                        //EditorGUILayout.PropertyField(EditorKeyPress,new GUIContent("Button 1 Unity Input Name"),true);
                        myTarget.ButtonIndexes[0] = EditorGUILayout.Popup("\tButton 1 Input To Use", myTarget.ButtonIndexes[0], myTarget.ListOfInputs.ToArray());
                        EditorGUILayout.BeginHorizontal();

                        myTarget.ButtonIsAxisCheck[0] = EditorGUILayout.Toggle(new GUIContent("\tButton 1 Is Axis", "Off -> Input is a button press On -> Input is an Axis"), myTarget.ButtonIsAxisCheck[0], GUILayout.MaxWidth(250));

                        if (myTarget.ButtonIsAxisCheck[0])
                        {
                            EditorGUIUtility.labelWidth = 100;
                            myTarget.ButtonAxisDetection[0] = EditorGUILayout.Popup("Direction", myTarget.ButtonAxisDetection[0], options);
                            myTarget.ButtonAxisThresholds[0] = EditorGUILayout.FloatField("Thresold", myTarget.ButtonAxisThresholds[0]);
                            EditorGUIUtility.labelWidth = 0;
                        }

                        EditorGUILayout.EndHorizontal();
                        if (myTarget.ButtonIsAxisCheck[0] && myTarget.EnableButtonFail)
                        {
                            EditorGUILayout.BeginHorizontal();
                            myTarget.AlternateAxisFailure[0] = EditorGUILayout.Toggle(new GUIContent("\tAlternate Axis Failure", "Choose Another Axis that will also fail the QTE"), myTarget.AlternateAxisFailure[0], GUILayout.MaxWidth(250));
                            if (myTarget.AlternateAxisFailure[0])
                            {
                                EditorGUIUtility.labelWidth = 100;
                                myTarget.FailureAxis[0] = EditorGUILayout.Popup("Failure Axis", myTarget.FailureAxis[0], myTarget.ListOfInputs.ToArray());
                                EditorGUIUtility.labelWidth = 0;
                            }
                            EditorGUILayout.EndHorizontal();
                        }

                    }
                    else
                    {
                        myTarget.ButtonKeyPresses[0] = EditorGUILayout.TextField(new GUIContent("\tButton 1 Keyboard Key"), myTarget.ButtonKeyPresses[0]);
                    }
                    serializedObject.ApplyModifiedProperties();

                }
                else
                {
                    EditorGUILayout.BeginHorizontal();
                    // EditorGUILayout.PrefixLabel(" ");

                    EditorStyles.foldout.padding.right = 5;
                     SerializedProperty tps = serializedObject.FindProperty("ArrayOfRandomButtons1");
                    string newLabel = "";

                    if (myTarget.UseInputs)
                    {
                        newLabel = "\tList of Random Inputs 1";
                    }
                    else
                    {
                        newLabel = "\tList of Random Keys 1";
                    }
                    EditorGUILayout.PropertyField(tps, new GUIContent(newLabel, "If Array is left at 0, a random button will be chosen from all available buttons. If filled up with names of Keys or Inputs, a random one will be chosen from that list instead."), true );
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }
                if (QTEtype.enumValueIndex == 1 || QTEtype.enumValueIndex == 2 || QTEtype.enumValueIndex == 3)
                {
                    serializedObject.ApplyModifiedProperties();
                    EditorGUILayout.LabelField("Button 2", EditorStyles.boldLabel);
                    myTarget.UseRandomButtons[1] = EditorGUILayout.Toggle(new GUIContent("\tRandomize", "Check true if you want to use an Randomly Generated button"), myTarget.UseRandomButtons[1]);

                    serializedObject.ApplyModifiedProperties();

                    if (!myTarget.UseRandomButtons[1])
                    {

                        if (myTarget.UseInputs)
                        {
                            //EditorKeyPress2 = serializedObject.FindProperty ("Button2KeyPress");
                            //EditorGUILayout.PropertyField(EditorKeyPress2,new GUIContent("Button 2 Unity Input Name"),true);
                            myTarget.ButtonIndexes[1] = EditorGUILayout.Popup("\tButton 2 Input To Use", myTarget.ButtonIndexes[1], myTarget.ListOfInputs.ToArray());
                            myTarget.ButtonIsAxisCheck[1] = EditorGUILayout.Toggle(new GUIContent("\tButton 2 Is Axis", "Off -> Input is a button press On -> Input is an Axis"), myTarget.ButtonIsAxisCheck[1]);

                            if (myTarget.ButtonIsAxisCheck[1])
                            {
                                EditorGUIUtility.labelWidth = 100;
                                myTarget.ButtonAxisDetection[1] = EditorGUILayout.Popup("Direction", myTarget.ButtonAxisDetection[1], options);
                                myTarget.ButtonAxisThresholds[1] = EditorGUILayout.FloatField("Thresold", myTarget.ButtonAxisThresholds[1]);
                                EditorGUIUtility.labelWidth = 0;
                            }
                        }
                        else
                        {
                            //EditorKeyPress2 = serializedObject.FindProperty ("Button2KeyPress");
                            //EditorGUILayout.PropertyField (EditorKeyPress2, new GUIContent ("\tButton 2 Keyboard Key"), true);
                            myTarget.ButtonKeyPresses[1] = EditorGUILayout.TextField(new GUIContent("\tButton 2 Keyboard Key"), myTarget.ButtonKeyPresses[1]);
                        }
                        serializedObject.ApplyModifiedProperties();
                    }
                    else
                    {
                        SerializedProperty tps2 = serializedObject.FindProperty("ArrayOfRandomButtons2");
                        string newLabel = "";

                        if (myTarget.UseInputs)
                        {
                            newLabel = "\tList of Random Inputs 1";
                        }
                        else
                        {
                            newLabel = "\tList of Random Keys 1";
                        }
                        EditorGUILayout.PropertyField(tps2, new GUIContent(newLabel, "If Array is left at 0, a random button will be chosen from all available buttons. If filled up with names of Keys or Inputs, a random one will be chosen from that list instead."), true);

                        EditorGUILayout.Space();
                    }
                }
                if (QTEtype.enumValueIndex == 2 || QTEtype.enumValueIndex == 3)
                {
                    EditorGUILayout.LabelField("Button 3", EditorStyles.boldLabel);
                    myTarget.UseRandomButtons[2] = EditorGUILayout.Toggle(new GUIContent("\tRandomize", "Check true if you want to use an Randomly Generated button"), myTarget.UseRandomButtons[2]);

                    if (!myTarget.UseRandomButtons[2])
                    {

                        if (myTarget.UseInputs)
                        {
                            //EditorKeyPress3 = serializedObject.FindProperty ("Button3KeyPress");
                            //EditorGUILayout.PropertyField(EditorKeyPress3,new GUIContent("Button 3 Unity Input Name"),true);
                            myTarget.ButtonIndexes[2] = EditorGUILayout.Popup("\tButton 3 Input To Use", myTarget.ButtonIndexes[2], myTarget.ListOfInputs.ToArray());
                            myTarget.ButtonIsAxisCheck[2] = EditorGUILayout.Toggle(new GUIContent("\tButton 3 Is Axis", "Off -> Input is a button press On -> Input is an Axis"), myTarget.ButtonIsAxisCheck[2]);

                            if (myTarget.ButtonIsAxisCheck[2])
                            {
                                EditorGUIUtility.labelWidth = 100;
                                myTarget.ButtonAxisDetection[2] = EditorGUILayout.Popup("Direction", myTarget.ButtonAxisDetection[2], options);
                                myTarget.ButtonAxisThresholds[2] = EditorGUILayout.FloatField("Thresold", myTarget.ButtonAxisThresholds[2]);
                                EditorGUIUtility.labelWidth = 0;
                            }
                        }
                        else
                        {
                            //EditorKeyPress3 = serializedObject.FindProperty ("Button3KeyPress");
                            //EditorGUILayout.PropertyField (EditorKeyPress3, new GUIContent ("\tButton 3 Keyboard Key"), true);
                            myTarget.ButtonKeyPresses[2] = EditorGUILayout.TextField(new GUIContent("\tButton 3 Keyboard Key"), myTarget.ButtonKeyPresses[2]);
                        }
                        serializedObject.ApplyModifiedProperties();
                    }
                    else
                    {
                        SerializedProperty tps3 = serializedObject.FindProperty("ArrayOfRandomButtons3");
                        string newLabel = "";

                        if (myTarget.UseInputs)
                        {
                            newLabel = "\tList of Random Inputs 1";
                        }
                        else
                        {
                            newLabel = "\tList of Random Keys 1";
                        }
                        EditorGUILayout.PropertyField(tps3, new GUIContent(newLabel, "If Array is left at 0, a random button will be chosen from all available buttons. If filled up with names of Keys or Inputs, a random one will be chosen from that list instead."), true);
                        EditorGUILayout.Space();
                    }
                }
                if (QTEtype.enumValueIndex == 3)
                {
                    EditorGUILayout.LabelField("Button 4", EditorStyles.boldLabel);
                    myTarget.UseRandomButtons[3] = EditorGUILayout.Toggle(new GUIContent("\tRandomize", "Check true if you want to use an Randomly Generated button"), myTarget.UseRandomButtons[3]);

                    if (!myTarget.UseRandomButtons[3])
                    {
                        if (myTarget.UseInputs)
                        {
                            //EditorKeyPress4 = serializedObject.FindProperty ("Button4KeyPress");
                            //EditorGUILayout.PropertyField(EditorKeyPress4,new GUIContent("Button 4 Unity Input Name"),true);
                            myTarget.ButtonIndexes[3] = EditorGUILayout.Popup("\tButton 4 Input To Use", myTarget.ButtonIndexes[3], myTarget.ListOfInputs.ToArray());
                            myTarget.ButtonIsAxisCheck[3] = EditorGUILayout.Toggle(new GUIContent("\tButton 4 Is Axis", "Off -> Input is a button press On -> Input is an Axis"), myTarget.ButtonIsAxisCheck[3]);

                            if (myTarget.ButtonIsAxisCheck[3])
                            {
                                EditorGUIUtility.labelWidth = 100;
                                myTarget.ButtonAxisDetection[3] = EditorGUILayout.Popup("Direction", myTarget.ButtonAxisDetection[3], options);
                                myTarget.ButtonAxisThresholds[3] = EditorGUILayout.FloatField("Thresold", myTarget.ButtonAxisThresholds[3]);
                                EditorGUIUtility.labelWidth = 0;
                            }
                        }
                        else
                        {
                            //EditorKeyPress4 = serializedObject.FindProperty ("Button4KeyPress");
                            //EditorGUILayout.PropertyField (EditorKeyPress4, new GUIContent ("\tButton 4 Keyboard Key"), true);
                            myTarget.ButtonKeyPresses[3] = EditorGUILayout.TextField(new GUIContent("\tButton 4 Keyboard Key"), myTarget.ButtonKeyPresses[3]);
                        }
                        serializedObject.ApplyModifiedProperties();
                    }
                    else
                    {
                        SerializedProperty tps4 = serializedObject.FindProperty("ArrayOfRandomButtons4");

                        string newLabel = "";

                        if (myTarget.UseInputs)
                        {
                            newLabel = "\tList of Random Inputs 1";
                        }
                        else
                        {
                            newLabel = "\tList of Random Keys 1";
                        }

                        EditorGUILayout.PropertyField(tps4, new GUIContent(newLabel, "If Array is left at 0, a random button will be chosen from all available buttons. If filled up with names of Keys or Inputs, a random one will be chosen from that list instead."), true);
                        EditorGUILayout.Space();
                    }
                }
            }

            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.Space();
        TimerOptions = EditorGUILayout.Foldout(TimerOptions, "Timer Options");
        if (TimerOptions)
        {
            EditorGUILayout.BeginVertical("Box");
            if (QTEtype.enumValueIndex == 4)
            {
                SerializedProperty EditorNoTimer = serializedObject.FindProperty("NoTimer");
                EditorGUILayout.PropertyField(EditorNoTimer, new GUIContent("Disable Timer Fail", "If checked, the QTE will not automatically end after 'Count Down Time' expires, instead the button remains until he succeeds, but if he stops pressing the button after the length of 'Count Down Time' he will fail."), true);
            }

            SerializedProperty EditorTimerDelayTime = serializedObject.FindProperty("TimerDelayTime");
            EditorGUILayout.PropertyField(EditorTimerDelayTime, new GUIContent("Delay Length:", "The time in seconds to Delay the QTE from happening after it's beein triggered"), true);

            myTarget.CountDownTimes[0] = EditorGUILayout.FloatField(new GUIContent("Timer:", "The time in seconds the player has to press the button before failing."), myTarget.CountDownTimes[0]);

            if (QTEtype.enumValueIndex == 1 || QTEtype.enumValueIndex == 2 || QTEtype.enumValueIndex == 3)
            {
                SerializedProperty EditorMultiTimer = serializedObject.FindProperty("MultiTimer");
                EditorGUILayout.PropertyField(EditorMultiTimer, new GUIContent("Timer Per Button"), true);
                if (EditorMultiTimer.boolValue)
                {

                    myTarget.CountDownTimes[1] = EditorGUILayout.FloatField(new GUIContent("\tButton 2 Timer:", "The time in seconds the player has to press the button before failing."), myTarget.CountDownTimes[1]);

                    if (QTEtype.enumValueIndex == 2 || QTEtype.enumValueIndex == 3)
                    {

                        myTarget.CountDownTimes[2] = EditorGUILayout.FloatField(new GUIContent("\tButton 3 Timer:", "The time in seconds the player has to press the button before failing."), myTarget.CountDownTimes[2]);


                    }
                    if (QTEtype.enumValueIndex == 3)
                    {
                        myTarget.CountDownTimes[3] = EditorGUILayout.FloatField(new GUIContent("\tButton 4 Timer:", "The time in seconds the player has to press the button before failing."), myTarget.CountDownTimes[3]);

                    }
                }
            }
            SerializedProperty EditorDelayTime = serializedObject.FindProperty("DelayTime");

            EditorGUILayout.PropertyField(EditorDelayTime, new GUIContent("Input Delay Length:", "The time in seconds a Delay between when the QTE appears, and when Input  starts being detected."), true);

            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.Space();

        DisplayOptions = EditorGUILayout.Foldout(DisplayOptions, "Display Options");
        if (DisplayOptions)
        {
            EditorGUILayout.BeginVertical("Box");
            myTarget.Hidden = EditorGUILayout.Toggle(new GUIContent("Hidden", "If true, No On screen Buttons will appear"), myTarget.Hidden);
            myTarget.FadeInUI = EditorGUILayout.Toggle(new GUIContent("Fade In", "If true, UI buttons will fade in"), myTarget.FadeInUI);

            if (myTarget.FadeInUI)
            {
                myTarget.FadeInTime = EditorGUILayout.FloatField(new GUIContent("Fade In Time", "Time the buttons fade in"), myTarget.FadeInTime);
            }

            if (QTEtype.enumValueIndex == 4)
            {
                myTarget.PulsateSpeed = EditorGUILayout.FloatField(new GUIContent("Pulsating Speed", "How fast the button pulsates in and out, 0 will disable this effect."), myTarget.PulsateSpeed);
                myTarget.PulsateFrequency = EditorGUILayout.FloatField(new GUIContent("Pulsate Frequency:", "The Strength of the Pulsating"), myTarget.PulsateFrequency);
            }

            myTarget.ButtonShaking[0] = EditorGUILayout.Toggle(new GUIContent("Shake 1st Button", "Button shakes back and forth"), myTarget.ButtonShaking[0]);


            if (myTarget.ButtonShaking[0])
            {
                myTarget.ButtonShakeOffests[0] = EditorGUILayout.FloatField(new GUIContent("\tShake Offset"), myTarget.ButtonShakeOffests[0]);
            }

            if (QTEtype.enumValueIndex == 1 || QTEtype.enumValueIndex == 2 || QTEtype.enumValueIndex == 3)
            {
                myTarget.ButtonShaking[1] = EditorGUILayout.Toggle(new GUIContent("Shake 2nd Button", "Button shakes back and forth"), myTarget.ButtonShaking[1]);
                if (myTarget.ButtonShaking[1])
                {
                    myTarget.ButtonShakeOffests[1] = EditorGUILayout.FloatField(new GUIContent("\tShake Offset"), myTarget.ButtonShakeOffests[1]);
                }
            }

            if (QTEtype.enumValueIndex == 2 || QTEtype.enumValueIndex == 3)
            {

                myTarget.ButtonShaking[2] = EditorGUILayout.Toggle(new GUIContent("Shake 3rd Button", "Button shakes back and forth"), myTarget.ButtonShaking[2]);
                if (myTarget.ButtonShaking[2])
                {
                    myTarget.ButtonShakeOffests[2] = EditorGUILayout.FloatField(new GUIContent("\tShake Offset"), myTarget.ButtonShakeOffests[2]);
                }
            }
            if (QTEtype.enumValueIndex == 3)
            {

                myTarget.ButtonShaking[3] = EditorGUILayout.Toggle(new GUIContent("Shake 4th Button", "Button shakes back and forth"), myTarget.ButtonShaking[3]);
                if (myTarget.ButtonShaking[3])
                {


                    myTarget.ButtonShakeOffests[3] = EditorGUILayout.FloatField(new GUIContent("\tShake Offset"), myTarget.ButtonShakeOffests[3]);
                }
            }

            myTarget.OverideButtonPosition = EditorGUILayout.Toggle(new GUIContent("Overide Button Position", "Overide the position of the buttons, buttons will be moved to the new positions"), myTarget.OverideButtonPosition);
            if (myTarget.OverideButtonPosition)
            {

                myTarget.ButtonPositions[0] = EditorGUILayout.Vector3Field(new GUIContent("	Button 1 Position:", "Distance in Units away from the center of the Canvas"), myTarget.ButtonPositions[0]);

                if (QTEtype.enumValueIndex == 1 || QTEtype.enumValueIndex == 2 || QTEtype.enumValueIndex == 3)
                {

                    myTarget.ButtonPositions[1] = EditorGUILayout.Vector3Field(new GUIContent("	Button 2 Position:", "Distance in Units away from the center of the Canvas"), myTarget.ButtonPositions[1]);
                }
                if (QTEtype.enumValueIndex == 2 || QTEtype.enumValueIndex == 3)
                {

                    myTarget.ButtonPositions[2] = EditorGUILayout.Vector3Field(new GUIContent("	Button 3 Position:", "Distance in Units away from the center of the Canvas"), myTarget.ButtonPositions[2]);

                }
                if (QTEtype.enumValueIndex == 3)
                {
                    myTarget.ButtonPositions[3] = EditorGUILayout.Vector3Field(new GUIContent("	Button 4 Position:", "Distance in Units away from the center of the Canvas"), myTarget.ButtonPositions[3]);
                }
            }
            myTarget.RandomizeButtonPositions[0] = EditorGUILayout.Toggle(new GUIContent("Randomize Button 1 Position", "Button position onscreen will be randomized, but will never appear off screen"), myTarget.RandomizeButtonPositions[0]);
            if (QTEtype.enumValueIndex == 1 || QTEtype.enumValueIndex == 2 || QTEtype.enumValueIndex == 3)
            {
                myTarget.RandomizeButtonPositions[1] = EditorGUILayout.Toggle(new GUIContent("Randomize Button 2 Position", ""), myTarget.RandomizeButtonPositions[1]);
            }
            if (QTEtype.enumValueIndex == 2 || QTEtype.enumValueIndex == 3)
            {
                myTarget.RandomizeButtonPositions[2] = EditorGUILayout.Toggle(new GUIContent("Randomize Button 3 Position", ""), myTarget.RandomizeButtonPositions[2]);
            }
            if (QTEtype.enumValueIndex == 3)
            {
                myTarget.RandomizeButtonPositions[3] = EditorGUILayout.Toggle(new GUIContent("Randomize Button 4 Position", ""), myTarget.RandomizeButtonPositions[3]);
            }
            if (myTarget.RandomizeButtonPositions[0] || myTarget.RandomizeButtonPositions[1] || myTarget.RandomizeButtonPositions[2] || myTarget.RandomizeButtonPositions[3])
            {
                myTarget.CanvasPadding = EditorGUILayout.Vector2Field(new GUIContent("Canvas Padding", ""), myTarget.CanvasPadding);
            }
            EditorGUILayout.EndVertical();
        }

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(myTarget);
    }


    public static List<string> GetInputAxis()
    {
        var allAxis = new List<string>();
        var serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        var axesProperty = serializedObject.FindProperty("m_Axes");
        //var NegProperty = negativeButton
        axesProperty.Next(true);
        axesProperty.Next(true);
        while (axesProperty.Next(false))
        {
            SerializedProperty axis = axesProperty.Copy();
            axis.Next(true);
            allAxis.Add(axis.stringValue);
        }
        return allAxis;
    }

}