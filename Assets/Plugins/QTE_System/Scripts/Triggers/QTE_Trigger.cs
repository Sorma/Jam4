/* QTE Trigger, Stores all informaiton related to a QTE and looks for the conditions to trigger it, passes the info off to QTE_main singleton to cause the QTE to happen*/

using UnityEngine;
using System.Collections.Generic;


public class QTE_Trigger : QTE_BaseClass
{
	//The override Canvas
	public GameObject Canvas;
	public bool TriggerEnabled = true;

    //If false, QTE can only be triggered once
	public bool Repeatable = false;

	public string[] myNewDirections = new string[]{ "Positive", "Negitive" };	

	public List<string> ListOfInputs = new List<string> ();

	public int[] ButtonIndexes = new int[4];

	public bool MultiTimer;
	public float DelayTime;
	public float TimerDelayTime;

	public string[] ButtonKeyPresses = new string[4];
    
    public bool[] UseRandomButtons = new bool[4];

	public string[] ArrayOfRandomButtons1, ArrayOfRandomButtons2, ArrayOfRandomButtons3, ArrayOfRandomButtons4;

	public int[] RandomNumbers = new int[4];

	private Object[] textures;
	
	private Collider Bcoll;
	private bool IsCollider;
	public bool EnableTouch;

	public bool[] RandomSwipeDirections = new bool[4];
    
	public enum OPTIONS
	{
		Single,
		Dual,
		Tri,
		Quad,
		Mash,
	}



	public OPTIONS QTEtype = OPTIONS.Single;

    //public QTE_BaseClass.TouchTypes TouchType, TouchType2, TouchType3, TouchType4;

	//public bool TouchHit;
    //public TouchInfo[] Touches;

   // bool direction;

    void Start ()
	{
		
		//Check to see if the attached gameobject has a Collider or not
		Bcoll = GetComponent<Collider> ();
		
		if (Bcoll != null && Bcoll.isTrigger) {
			IsCollider = true;
		} else {
			IsCollider = false;
		}
		
		if (!IsCollider) {
			TriggerEnabled = false;
			Repeatable = false;
		}

		if (Canvas != null) {
			Canvas.transform.Find ("QTE_UI/Button1").gameObject.SetActive (false);
			Canvas.transform.Find ("QTE_UI/Button2").gameObject.SetActive (false);
			Canvas.transform.Find ("QTE_UI/Button3").gameObject.SetActive (false);
			Canvas.transform.Find ("QTE_UI/Button4").gameObject.SetActive (false);
		}
	}
	
	//Called by the user when QTE is being used manually.
	public void TriggerQTE ()
	{
		TriggerEnabled = true;
	}

	void PrintError (string button)
	{
		Debug.LogError ("QTE System: An Image for the Key or Input '" + button + "' Could not be found, please make one and put it in the Resources Folder.");
	}

    //Function that takes the requested button information, and returns the sprite
	Sprite GetButtonSprite (bool isAxis, int direction, string name)
	{
		Sprite mySprite;

		if (isAxis) {
           // Debug.Log("Is An Axis");		
            if (direction == 0) {
				mySprite = Resources.Load ("QTE_System/Textures/InputsSprite/QTE_" + name + "_Pos", typeof(Sprite)) as Sprite;
               // Debug.Log("QTE_System/Textures/InputsSprite/QTE_" + name + "_Pos");
            } else {
				mySprite = Resources.Load ("QTE_System/Textures/InputsSprite/QTE_" + name + "_Neg", typeof(Sprite)) as Sprite;
               // Debug.Log("QTE_System/Textures/InputsSprite/QTE_" + name + "_Neg");
            }
           
        } else {
			//Debug.Log("Not An Axis");
			mySprite = Resources.Load ("QTE_System/Textures/InputsSprite/QTE_" + name, typeof(Sprite)) as Sprite;
		}

		return mySprite;
	}
    
    //This function strips out text from the filename of a sprite, to retrieve the base name of the input/key it is for.
    string StripInputName(string myString)
    {
        string RemovePos = myString.Replace("_Pos", "");
        string RemoveNeg = RemovePos.Replace("_Neg", "");
        string result = RemoveNeg.Replace("QTE_", "");
        return result;
    }

    //This fuction Detects the direction of an AXis, and set values.
    void DetectAxisDirection(int index, string filename)
    {
        if (filename.Contains("_Pos") || filename.Contains("_Neg"))
        {
            ButtonIsAxisCheck[index] = true;
            if (filename.Contains("_Pos"))
            {
                ButtonAxisDetection[index] = 0;
            }

            if (filename.Contains("_Neg"))
            {
                ButtonAxisDetection[index] = 1;
            }
        }
        else
        {
            ButtonIsAxisCheck[index] = false;
        }
    }

	void QTE ()
	{
		if (UseInputs) {
			ButtonKeyPresses[0] = ListOfInputs [ButtonIndexes[0]];
			ButtonKeyPresses[1] = ListOfInputs [ButtonIndexes[1]];
			ButtonKeyPresses[2] = ListOfInputs [ButtonIndexes[2]];
			ButtonKeyPresses[3] = ListOfInputs [ButtonIndexes[3]];

            FailureInputName[0] = ListOfInputs[FailureAxis[0]];
            FailureInputName[1] = ListOfInputs[FailureAxis[1]];
            FailureInputName[2] = ListOfInputs[FailureAxis[2]];
            FailureInputName[3] = ListOfInputs[FailureAxis[3]];
        }

		if (TriggerEnabled && !QTE_main.Singleton.QTEactive) {
			//Set the varibles of the Main Script to the values provided by the instance of this script.

			if (Canvas != null) {
				QTE_main.Singleton.OtherCanvas = Canvas;
			} else {
				QTE_main.Singleton.OtherCanvas = null;
			}

			QTE_main.Singleton.OverideButtonPosition = OverideButtonPosition;

			if (OverideButtonPosition) {
				QTE_main.Singleton.ButtonPositions[0] = ButtonPositions[0];
				QTE_main.Singleton.ButtonPositions[1] = ButtonPositions[1];
				QTE_main.Singleton.ButtonPositions[2] = ButtonPositions[2];
				QTE_main.Singleton.ButtonPositions[3] = ButtonPositions[3];
			}

			if (EnableTouch) {
			
			} else {
				//if using a random button for option 1
				if (UseRandomButtons[0]) {
                    if (ArrayOfRandomButtons1.Length == 0)
                    {
                        //load all sprites into an array to find out how many buttons are avalible
                        if (!UseInputs)
                        {
                            textures = Resources.LoadAll("QTE_System/Textures/KeyboardSprite", typeof(Sprite));
                        }
                        else
                        {
                            textures = Resources.LoadAll("QTE_System/Textures/InputsSprite", typeof(Sprite));
                        }
                        //gererate a random number 
                        RandomNumbers[0] = Random.Range(0, textures.Length);

                        //get the name of the texture at the random numbers index
                        string filename = textures[RandomNumbers[0]].name;

                        //If the filename for the button contains "_Pos" or "_Neg", that means the input is an axis, and not a button.
                        //So Set the Axis check value to true, and determine which direction the AXis wants

                        DetectAxisDirection(0, filename);

                        string result = StripInputName(filename);

                        QTE_main.Singleton.KeyPress = result;    

                        if (!UseInputs)
                        {
                            UISprites[0] = Resources.Load("QTE_System/Textures/KeyboardSprite/QTE_" + result, typeof(Sprite)) as Sprite;
                        }
                        else
                        {
                            UISprites[0] = GetButtonSprite(ButtonIsAxisCheck[0], ButtonAxisDetection[0], result);
                        }
                      
                    }
                    else
                    {
                        var randomNumber = Random.Range(0, ArrayOfRandomButtons1.Length);
                        string filename = ArrayOfRandomButtons1[randomNumber].ToString();

                        DetectAxisDirection(0, filename);

                        string result = StripInputName(filename);

                        QTE_main.Singleton.KeyPress = result;

                        if (UseInputs)
                        {
                            UISprites[0] = GetButtonSprite(ButtonIsAxisCheck[0], ButtonAxisDetection[0], result);
                            QTE_main.Singleton.UISprites[0] = UISprites[0];
                        }
                        else
                        {
                            UISprites[0] = Resources.Load("QTE_System/Textures/KeyboardSprite/QTE_" + result, typeof(Sprite)) as Sprite;
                            QTE_main.Singleton.UISprites[0] = UISprites[0];
                        }
                    }

				} else {
					QTE_main.Singleton.KeyPress = ButtonKeyPresses[0];

					if (!UseInputs) {
						UISprites[0] = Resources.Load ("QTE_System/Textures/KeyboardSprite/QTE_" + ButtonKeyPresses[0], typeof(Sprite)) as Sprite;
					} else {
						UISprites[0] = GetButtonSprite (ButtonIsAxisCheck[0], ButtonAxisDetection[0], ButtonKeyPresses[0]);
					}

				}

				if (QTEtype == OPTIONS.Dual || QTEtype == OPTIONS.Tri || QTEtype == OPTIONS.Quad) {
					//if using a random button for option 2
					if (UseRandomButtons[1]) {
						if (ArrayOfRandomButtons2.Length == 0) {
							if (!UseInputs) {				
								textures = Resources.LoadAll ("QTE_System/Textures/KeyboardSprite", typeof(Sprite));
							} else {
								textures = Resources.LoadAll ("QTE_System/Textures/InputsSprite", typeof(Sprite));
							}
							//gererate a random number
							RandomNumbers[1] = GenerateRandomExculsive (textures, RandomNumbers[0]);

                            string filename2 = textures [RandomNumbers[1]].name;

                           DetectAxisDirection(1, filename2);
                            string result2 = StripInputName(filename2);

                            QTE_main.Singleton.KeyPress2 = result2;

							if (!UseInputs) {
								UISprites[1] = Resources.Load ("QTE_System/Textures/KeyboardSprite/QTE_" + result2, typeof(Sprite)) as Sprite;
							} else {
								UISprites[1] = GetButtonSprite (ButtonIsAxisCheck[1], ButtonAxisDetection[1], result2);
								//UISprites[1] = Resources.Load("QTE_System/Textures/InputsSprite/QTE_" + result2,typeof(Sprite) ) as Sprite;
							}	
						} else {
							var randomNumber = Random.Range (0, ArrayOfRandomButtons2.Length);
							string filename2 = ArrayOfRandomButtons2 [randomNumber].ToString ();

                            DetectAxisDirection(1, filename2);
                            string result = StripInputName(filename2);

                            QTE_main.Singleton.KeyPress2 = result;

							if (UseInputs) {	
								//UISprites[1] = Resources.Load("QTE_System/Textures/InputsSprite/QTE_" + result,typeof(Sprite) ) as Sprite;
								UISprites[1] = GetButtonSprite (ButtonIsAxisCheck[1], ButtonAxisDetection[1], result);
							} else {					
								UISprites[1] = Resources.Load ("QTE_System/Textures/KeyboardSprite/QTE_" + result, typeof(Sprite)) as Sprite;
								//QTE_texture.texture = OnScreenTexture;
							}
						}
					} else {
						QTE_main.Singleton.KeyPress2 = ButtonKeyPresses[1];

						if (!UseInputs) {
							UISprites[1] = Resources.Load ("QTE_System/Textures/KeyboardSprite/QTE_" + ButtonKeyPresses[1], typeof(Sprite)) as Sprite;
						} else {
							UISprites[1] = Resources.Load ("QTE_System/Textures/InputsSprite/QTE_" + ButtonKeyPresses[1], typeof(Sprite)) as Sprite;
						}

					}
				}

				if (QTEtype == OPTIONS.Tri || QTEtype == OPTIONS.Quad) {
					//if using a random button for option 3
					if (UseRandomButtons[2]) {
						if (ArrayOfRandomButtons3.Length == 0) {
							if (!UseInputs) {				
								textures = Resources.LoadAll ("QTE_System/TexturesSprite/Keyboard", typeof(Sprite));
							} else {
								textures = Resources.LoadAll ("QTE_System/TexturesSprite/Inputs", typeof(Sprite));
							}
							//gererate a random number
							RandomNumbers[2] = GenerateRandomExculsive2 (textures, RandomNumbers[0], RandomNumbers[1]);

							string filename = textures [RandomNumbers[2]].name;

                            DetectAxisDirection(2, filename);
                            string result = StripInputName(filename);

                            QTE_main.Singleton.KeyPress3 = result;

							if (!UseInputs) {
								UISprites[2] = Resources.Load ("QTE_System/Textures/KeyboardSprite/QTE_" + result, typeof(Sprite)) as Sprite;
							} else {
								//UISprites[2] = Resources.Load("QTE_System/Textures/InputsSprite/QTE_" + result,typeof(Sprite) ) as Sprite;
								UISprites[2] = GetButtonSprite (ButtonIsAxisCheck[2], ButtonAxisDetection[2], result);
							}
						} else {
							var randomNumber = Random.Range (0, ArrayOfRandomButtons3.Length);
							string filename = ArrayOfRandomButtons3 [randomNumber].ToString ();

                            DetectAxisDirection(2, filename);
                            string result = StripInputName(filename);

                            QTE_main.Singleton.KeyPress3 = result;

							if (UseInputs) {	
								//UISprites[2] = Resources.Load("QTE_System/Textures/InputsSprite/QTE_" + result,typeof(Sprite) ) as Sprite;
								UISprites[2] = GetButtonSprite (ButtonIsAxisCheck[2], ButtonAxisDetection[2], result);
							} else {					
								UISprites[2] = Resources.Load ("QTE_System/Textures/KeyboardSprite/QTE_" + result, typeof(Sprite)) as Sprite;
							}
						}
					} else {
						QTE_main.Singleton.KeyPress3 = ButtonKeyPresses[2];

						if (!UseInputs) {
							UISprites[2] = Resources.Load ("QTE_System/Textures/KeyboardSprite/QTE_" + ButtonKeyPresses[2], typeof(Sprite)) as Sprite;
						} else {
							UISprites[2] = Resources.Load ("QTE_System/Textures/InputsSprite/QTE_" + ButtonKeyPresses[2], typeof(Sprite)) as Sprite;

						}

					}
				}

				if (QTEtype == OPTIONS.Quad) {
					//if using a random button for option 3
					if (UseRandomButtons[3]) {
						if (ArrayOfRandomButtons4.Length == 0) {
							if (!UseInputs) {				
								textures = Resources.LoadAll ("QTE_System/TexturesSprite/KeyboardSprite", typeof(Sprite));
							} else {
								textures = Resources.LoadAll ("QTE_System/TexturesSprite/InputsSprite", typeof(Sprite));
							}
							//gererate a random number
							RandomNumbers[3] = GenerateRandomExculsive3 (textures, RandomNumbers[0], RandomNumbers[1], RandomNumbers[2]);		

							string filename = textures [RandomNumbers[3]].name;

                            DetectAxisDirection(3, filename);
                            string result = StripInputName(filename);

                            QTE_main.Singleton.KeyPress4 = result;

							if (!UseInputs) {
								UISprites[3] = Resources.Load ("QTE_System/Textures/KeyboardSprite/QTE_" + result, typeof(Sprite)) as Sprite;
							} else {
								//UISprites[3] = Resources.Load("QTE_System/Textures/InputsSprite/QTE_" + result,typeof(Sprite) ) as Sprite;
								UISprites[3] = GetButtonSprite (ButtonIsAxisCheck[3], ButtonAxisDetection[3], result);
							}	
						} else {
							var randomNumber = Random.Range (0, ArrayOfRandomButtons4.Length);
							string filename = ArrayOfRandomButtons4 [randomNumber].ToString ();

                            DetectAxisDirection(3, filename);
                            string result = StripInputName(filename);

                            QTE_main.Singleton.KeyPress4 = result;

							if (UseInputs) {	
								//UISprites[3] = Resources.Load("QTE_System/Textures/InputsSprite/QTE_" + result,typeof(Sprite) ) as Sprite;
								UISprites[3] = GetButtonSprite (ButtonIsAxisCheck[3], ButtonAxisDetection[3], result);
							} else {					
								UISprites[3] = Resources.Load ("QTE_System/Textures/KeyboardSprite/QTE_" + result, typeof(Sprite)) as Sprite;
							}
						}
					} else {
						QTE_main.Singleton.KeyPress4 = ButtonKeyPresses[3];

						if (!UseInputs) {
							UISprites[3] = Resources.Load ("QTE_System/Textures/KeyboardSprite/QTE_" + ButtonKeyPresses[3], typeof(Sprite)) as Sprite;
						} else {
							UISprites[3] = Resources.Load ("QTE_System/Textures/InputsSprite/QTE_" + ButtonKeyPresses[3], typeof(Sprite)) as Sprite;
						}

					}
				}

				if (QTEtype == OPTIONS.Dual) {
					QTE_main.Singleton.DualTrigger = true;
				} else if (QTEtype == OPTIONS.Tri) {
					QTE_main.Singleton.TriTrigger = true;
				} else if (QTEtype == OPTIONS.Quad) {
					QTE_main.Singleton.QuadTrigger = true;
				} else if (QTEtype == OPTIONS.Mash) {
					QTE_main.Singleton.Mashable = true;
					QTE_main.Singleton.NoTimer = NoTimer;
					QTE_main.Singleton.PulsateSpeed = PulsateSpeed;
					QTE_main.Singleton.PulsateFrequency = PulsateFrequency;
					QTE_main.Singleton.tolerance = tolerance;
				}
		
				QTE_main.Singleton.UseInputs = UseInputs;

                QTE_main.Singleton.FailureInputName[0] = FailureInputName[0];
                QTE_main.Singleton.FailureInputName[1] = FailureInputName[1];
                QTE_main.Singleton.FailureInputName[2] = FailureInputName[2];
                QTE_main.Singleton.FailureInputName[3] = FailureInputName[3];

                //These booleans determine if the Chosen input is an Axis or not
                QTE_main.Singleton.ButtonIsAxisCheck[0] = ButtonIsAxisCheck[0];
				QTE_main.Singleton.ButtonIsAxisCheck[1] = ButtonIsAxisCheck[1];
				QTE_main.Singleton.ButtonIsAxisCheck[2] = ButtonIsAxisCheck[2];
				QTE_main.Singleton.ButtonIsAxisCheck[3] = ButtonIsAxisCheck[3];

                //These floats are the thresolds axes have to pass to succeed.
				QTE_main.Singleton.ButtonAxisThresholds[0] = ButtonAxisThresholds[0];
				QTE_main.Singleton.ButtonAxisThresholds[1] = ButtonAxisThresholds[1];
				QTE_main.Singleton.ButtonAxisThresholds[2] = ButtonAxisThresholds[2];
				QTE_main.Singleton.ButtonAxisThresholds[3] = ButtonAxisThresholds[3];

                //these floats are either set to 0 (Positive) or 1 (Negitive) to specifiy what Axis direction to look for
 				QTE_main.Singleton.ButtonAxisDetection[0] = ButtonAxisDetection[0];
				QTE_main.Singleton.ButtonAxisDetection[1] = ButtonAxisDetection[1];
				QTE_main.Singleton.ButtonAxisDetection[2] = ButtonAxisDetection[2];
				QTE_main.Singleton.ButtonAxisDetection[3] = ButtonAxisDetection[3];

                QTE_main.Singleton.FailureAxis[0] = FailureAxis[0];
                QTE_main.Singleton.FailureAxis[1] = FailureAxis[1];
                QTE_main.Singleton.FailureAxis[2] = FailureAxis[2];
                QTE_main.Singleton.FailureAxis[3] = FailureAxis[3];

                QTE_main.Singleton.AlternateAxisFailure[0] = AlternateAxisFailure[0];
                QTE_main.Singleton.AlternateAxisFailure[1] = AlternateAxisFailure[1];
                QTE_main.Singleton.AlternateAxisFailure[2] = AlternateAxisFailure[2];
                QTE_main.Singleton.AlternateAxisFailure[3] = AlternateAxisFailure[3];

                //if multi button QTE is chosen, pass along the sprites
                if (QTEtype == OPTIONS.Dual || QTEtype == OPTIONS.Tri || QTEtype == OPTIONS.Quad) {
					QTE_main.Singleton.UISprites[1] = UISprites[1];
				}
				if (QTEtype == OPTIONS.Tri || QTEtype == OPTIONS.Quad) {
					QTE_main.Singleton.UISprites[2] = UISprites[2];
				}
				if (QTEtype == OPTIONS.Quad) {
					QTE_main.Singleton.UISprites[3] = UISprites[3];
				}

				QTE_main.Singleton.EnableButtonFail = EnableButtonFail;

				if (MultiTimer) {
					QTE_main.Singleton.MultiTimerQuad = MultiTimer;
					if (QTEtype == OPTIONS.Dual || QTEtype == OPTIONS.Tri || QTEtype == OPTIONS.Quad) {
						QTE_main.Singleton.CountDownTimes[1] = CountDownTimes[1];
					}
					if (QTEtype == OPTIONS.Tri || QTEtype == OPTIONS.Quad) {
						QTE_main.Singleton.CountDownTimes[2] = CountDownTimes[2];
					}
					if (QTEtype == OPTIONS.Quad) {
						QTE_main.Singleton.CountDownTimes[3] = CountDownTimes[3];
					}
				}

				if (UISprites[0] != null) {	
					QTE_main.Singleton.UISprites[0] = UISprites[0];

				} else {
					PrintError (ButtonKeyPresses[0]);
				}

				if (QTEtype == OPTIONS.Dual || QTEtype == OPTIONS.Tri || QTEtype == OPTIONS.Quad) {
					if (UISprites[1] != null) {
						QTE_main.Singleton.UISprites[1] = UISprites[1];
					} else {
						PrintError (ButtonKeyPresses[1]);
					}
				}

				if (QTEtype == OPTIONS.Tri || QTEtype == OPTIONS.Quad) {
					if (UISprites[2] != null) {
						QTE_main.Singleton.UISprites[2] = UISprites[2];
					} else {
						PrintError (ButtonKeyPresses[2]);
					}
				}

				if (QTEtype == OPTIONS.Quad) {
					if (UISprites[3] != null) {
					} else {
						PrintError (ButtonKeyPresses[3]);
					}
				}
			}

            QTE_main.Singleton.FadeInUI = FadeInUI;
            QTE_main.Singleton.FadeInTime = FadeInTime;
            QTE_main.Singleton.IsTouch = EnableTouch;
			QTE_main.Singleton.TriggeringObject = this.gameObject;
			QTE_main.Singleton.Hidden = Hidden;
			QTE_main.Singleton.UISprites[0] = UISprites[0];
            //Debug.Log(UISprites[0].name);
            QTE_main.Singleton.RandomizeButtonPositions[0] = RandomizeButtonPositions[0];
			QTE_main.Singleton.RandomizeButtonPositions[1] = RandomizeButtonPositions[1];
			QTE_main.Singleton.RandomizeButtonPositions[2] = RandomizeButtonPositions[2];
			QTE_main.Singleton.RandomizeButtonPositions[3] = RandomizeButtonPositions[3];
			QTE_main.Singleton.CanvasPadding = CanvasPadding;
			QTE_main.Singleton.TriggerQTE (CountDownTimes[0], DelayTime, TimerDelayTime);
            			
			//Pass the Shaking info from this instance to the main script
			if (ButtonShaking[0]) {
				QTE_main.Singleton.ButtonShaking[0] = true;
				QTE_main.Singleton.ButtonShakeOffests[0] = ButtonShakeOffests[0];
			} else {
				QTE_main.Singleton.ButtonShaking[0]= false;
			}
			
			if (QTEtype == OPTIONS.Dual || QTEtype == OPTIONS.Tri || QTEtype == OPTIONS.Quad) {
				if (ButtonShaking[1]) {
					QTE_main.Singleton.ButtonShaking[1] = true;
					QTE_main.Singleton.ButtonShakeOffests[1] = ButtonShakeOffests[1];
				} else {
					QTE_main.Singleton.ButtonShaking[1] = false;
				}
			}
			
			if (QTEtype == OPTIONS.Tri || QTEtype == OPTIONS.Quad) {
				if (ButtonShaking[2]) {
					QTE_main.Singleton.ButtonShaking[2] = true;
					QTE_main.Singleton.ButtonShakeOffests[2] = ButtonShakeOffests[2];
				} else {
					QTE_main.Singleton.ButtonShaking[2] = false;
				}
			}
			
			if (QTEtype == OPTIONS.Quad) {
				if (ButtonShaking[3]) {
					QTE_main.Singleton.ButtonShaking[3] = true;
					QTE_main.Singleton.ButtonShakeOffests[3] = ButtonShakeOffests[3];
				} else {
					QTE_main.Singleton.ButtonShaking[3] = false;
				}
			}
		}
		
		//turn off this trigger
		if (!Repeatable && Bcoll != null || Bcoll == null) {
			TriggerEnabled = false;
		}
	}
	
	//functions for returing a random number, while exculding values
	public int GenerateRandomExculsive (Object[] thing, int exclude1)
	{
		int newDir;
	 
		newDir = Random.Range (0, thing.Length);
		while (newDir == exclude1) {
			newDir = Random.Range (0, thing.Length);
		}
	 
		return newDir;
	 
	}

	public int GenerateRandomExculsive2 (Object[] thing, int exclude1, int exclude2)
	{
		int newDir;
	 
		newDir = Random.Range (0, thing.Length);
		while (newDir == exclude1 || newDir == exclude2) {
			newDir = Random.Range (0, thing.Length);
		}
	 
		return newDir;
	 
	}

	public int GenerateRandomExculsive3 (Object[] thing, int exclude1, int exclude2, int exclude3)
	{
		int newDir;
	 
		newDir = Random.Range (0, thing.Length);
		while (newDir == exclude1 || newDir == exclude2 || newDir == exclude2) {
			newDir = Random.Range (0, thing.Length);
		}
	 
		return newDir;
	 
	}
	
	//if Attached to a Collider, trigger the QTE when he enters it
	void OnTriggerEnter (Collider collision)
	{ 
		if (IsCollider) {
			if (!collision.CompareTag ("Player")) {
				return;
			}
			QTE ();
		}
	}
	
	//if not attached to a Collider, then it will be triggered manually by the user.
	void Update ()
	{
		if (!IsCollider) {
			QTE ();
		}
	}
}
