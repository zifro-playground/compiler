using System;

namespace Compiler
{
	
	public class Variable : Logic, BoolType
	{
		public VariableTypes variableType;
		public string name;
		public bool isCalcVar = false;
		public bool isReturnCalcVar = false;

		private double numberValue;
		private bool boolValue;
		private string stringValue;

		public bool isForLoopVariable = false;

		public Variable(){
			variableType = VariableTypes.unknown;
		}

		public Variable(string name){
			this.name = name;
			this.word = name;
			variableType = VariableTypes.None;
		}
		public Variable(string name, string newValue, bool isCalcVar = false){
			this.name = name;
			this.word = name;
			variableType = VariableTypes.textString;
			stringValue = newValue;
			this.isCalcVar = isCalcVar;
		}
		public Variable(string name,double newValue, bool isCalcVar = false){
			this.name = name;
			this.word = name;
			variableType = VariableTypes.number;
			numberValue = newValue;
			this.isCalcVar = isCalcVar;
		}
		public Variable(string name,bool newValue, bool isCalcVar = false){
			this.name = name;
			this.word = name;
			variableType = VariableTypes.boolean;
			boolValue = newValue;
			this.isCalcVar = isCalcVar;
		}





		public void setValue(double newValue){
			variableType = VariableTypes.number;
			numberValue = newValue;
		}
		public void setValue(string newValue){
			variableType = VariableTypes.textString;
			stringValue = newValue;
		}
		public void setValue(bool newValue){
			variableType = VariableTypes.boolean;
			boolValue = newValue;
		}

		public void setValue(){
			variableType = VariableTypes.None;
		}


		public double getNumber(){
			return numberValue;
		}

		public bool getBool(){
			return boolValue;
		}

		public string getString(){
			return stringValue;
		}
	}
}

