// Conversation output generated using T3DConvoEditor
// Copyright © 2015 Roostertail Games
// Use of T3DConvoEditor and its output are governed by the MIT license.

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

//--- OBJECT WRITE BEGIN ---
new SimSet(output6){
	class = "Conversation";
	canSave = "1";
	canSaveDynamicFields = "1";

	new ScriptObject(output6_Conversation_End_000) {
		class = "ConversationEnd";
		canSave = "1";
		canSaveDynamicFields = "1";
			displayText = "Enter text";
			scriptMethod = "exit(5000);";
	};
	new ScriptObject(output6_Conversation_Start) {
		class = "ConversationStart";
		canSave = "1";
		canSaveDynamicFields = "1";
			numOutLinks = 1;
			outLink0 = output6_DefaultNodeName_0000;
	};
	new ScriptObject(output6_DefaultNodeName_0000) {
		class = "ConversationNode";
		canSave = "1";
		canSaveDynamicFields = "1";
			displayText = "Enter NPC text";
			numOutLinks = 1;
			button0next = output6_Conversation_End_000;
			button0 = "Enter player text";
	};
};
//--- OBJECT WRITE END ---
