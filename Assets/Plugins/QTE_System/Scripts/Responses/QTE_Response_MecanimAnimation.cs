/*This response script changes values of a Mecanim animator component, causing different animations to play based upon the considtions setup in the animator*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QTE_Response_MecanimAnimation : MonoBehaviour {
	
	public GameObject ObjectToAnimate;
	private Animator animator;



    public enum ParameterType {Float, Boolean, Int};
	
	[System.Serializable]
	public class MecanimParmeter {
		public ParameterType type;
		public string Name;
		public float ValueIfFloat;
		public bool ValueIfBool;
		public int ValueIfInt;
		//public Vector3 ValueIfVector;
	}	
	public MecanimParmeter[] Active;
	public MecanimParmeter[] Success1;
	public MecanimParmeter[] Success2;
	public MecanimParmeter[] Success3;
	public MecanimParmeter[] Success4;
	public MecanimParmeter[] Fail;

    //public AnimatorControllerParameter[] AllParameters;
    // public List<AnimatorControllerParameter> AllParameters = new List<AnimatorControllerParameter>();
    //public List<MyClass> ListOfAnimValues = new List<MyClass>();
    
    //public MecanimParmeter[] ListOfValues;


    // Use this for initialization
    void Start () {

        if (ObjectToAnimate == null)
        {
            animator = GetComponent<Animator>();
        }
        else
        {
            animator = ObjectToAnimate.GetComponent<Animator>();
        }

       /* AnimatorControllerParameter param;
        for (int i = 0; i < animator.parameters.Length; i++)
        {
            param = animator.parameters[i];
            Debug.Log("Parameter Name: " + param.name);

            if (param.type == AnimatorControllerParameterType.Bool)
            {
                Debug.Log("Default Bool: " + param.defaultBool);
            }
            else if (param.type == AnimatorControllerParameterType.Float)
            {
                Debug.Log("Default Float: " + param.defaultFloat);
            }
            else if (param.type == AnimatorControllerParameterType.Int)
            {
                Debug.Log("Default Int: " + param.defaultInt);
            }
        }*/


    }
	
	// Update is called once per frame
	void Update () {
		
	if(QTE_main.Singleton.TriggeringObject == this.gameObject){		
			
			//while the QTE is happening
		if(QTE_main.Singleton.QTEactive){
				foreach (MecanimParmeter item in Active)
				{
				    if(item.type == ParameterType.Float){
						animator.SetFloat(item.Name.ToString(), item.ValueIfFloat);
					}
					else if(item.type == ParameterType.Boolean){
						animator.SetBool(item.Name.ToString(), item.ValueIfBool);
					}
					else{
						animator.SetInteger(item.Name.ToString(), item.ValueIfInt);
					}
				}
			}

		if (QTE_main.Singleton.QTECompleted && !QTE_main.Singleton.QTEactive) {
			
			//if the QTE completed, and he succedded with option 1
		if(QTE_main.Singleton.succeeded){
				foreach (MecanimParmeter item in Success1)
				{
				    if(item.type == ParameterType.Float){
						animator.SetFloat(item.Name.ToString(), item.ValueIfFloat);
					}
					else if(item.type == ParameterType.Boolean){
						animator.SetBool(item.Name.ToString(), item.ValueIfBool);
					}
					else{
						animator.SetInteger(item.Name.ToString(), item.ValueIfInt);
					}
				}
			}
			
				//if the QTE completed, and he succedded with option 2 (Dual QTE only)
		if(QTE_main.Singleton.succeeded2){
				foreach (MecanimParmeter item in Success2)
				{
				    if(item.type == ParameterType.Float){
						animator.SetFloat(item.Name.ToString(), item.ValueIfFloat);
					}
					else if(item.type == ParameterType.Boolean){
						animator.SetBool(item.Name.ToString(), item.ValueIfBool);
					}
					else{
						animator.SetInteger(item.Name.ToString(), item.ValueIfInt);
					}
				}
			}
			
			//if the QTE completed, and he succedded with option 3 (Tri QTE only)
			if(QTE_main.Singleton.succeeded3){
				foreach (MecanimParmeter item in Success3)
				{
				    if(item.type == ParameterType.Float){
						animator.SetFloat(item.Name.ToString(), item.ValueIfFloat);
					}
					else if(item.type == ParameterType.Boolean){
						animator.SetBool(item.Name.ToString(), item.ValueIfBool);
					}
					else{
						animator.SetInteger(item.Name.ToString(), item.ValueIfInt);
					}
				}
			}
			
			//if the QTE completed, and he succedded with option 4 (Quad QTE only)
			if(QTE_main.Singleton.succeeded4){
				foreach (MecanimParmeter item in Success4)
				{
				    if(item.type == ParameterType.Float){
						animator.SetFloat(item.Name.ToString(), item.ValueIfFloat);
					}
					else if(item.type == ParameterType.Boolean){
						animator.SetBool(item.Name.ToString(), item.ValueIfBool);
					}
					else{
						animator.SetInteger(item.Name.ToString(), item.ValueIfInt);
					}
				}
			}
			
			//if the QTE completed, and he failed
			if (QTE_main.Singleton.QTE_Failed_WrongButton || QTE_main.Singleton.QTE_Failed_timer) {
				foreach (MecanimParmeter item in Fail)
				{
				    if(item.type == ParameterType.Float){
						animator.SetFloat(item.Name.ToString(), item.ValueIfFloat);
					}
					else if(item.type == ParameterType.Boolean){
						animator.SetBool(item.Name.ToString(), item.ValueIfBool);
					}
					else{
						animator.SetInteger(item.Name.ToString(), item.ValueIfInt);
					}
				}
			}
		}
	}
	
	}
}

