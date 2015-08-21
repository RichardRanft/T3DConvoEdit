//--- OBJECT WRITE BEGIN ---
new SimSet(TC_1) {
   class = "Conversation";
   canSave = "1";
   canSaveDynamicFields = "1";

   new ScriptObject(TC_1_node1) {
      class = "ConversationNode";
      canSave = "1";
      canSaveDynamicFields = "1";
         button2 = "That\'s great!";
         button2next=TC_1_node2;
		 button2Cmd="TC_1_node1.likeNode();";
         button6 = "That sucks!";
         button6next=TC_1_node3;
		 button6Cmd="TC_1_node1.hateNode();";
         displayText = "This is node one.  What do you think?";
   };
   new ScriptObject(TC_1_node2) {
      class = "ConversationNode";
      canSave = "1";
      canSaveDynamicFields = "1";
         button2 = "It\'s also great!";
		 button2Cmd="TC_1_node2.likeNode();";
         button3 = "See node 1 again";
         button3next=TC_1_node1;
		 button3Cmd="TC_1_node2.revisitNode();";
         button6 = "It sucks!";
		 button6Cmd="TC_1_node2.hateNode();";
         displayText = "Glad you liked node one.  This is node two.  What do you think?";
   };
   new ScriptObject(TC_1_node3) {
      class = "ConversationNode";
      canSave = "1";
      canSaveDynamicFields = "1";
         button2 = "It\'s great!";
		 button2Cmd="TC_1_node3.likeNode();";
         button3 = "See node 1 again";
         button3next=TC_1_node1;
		 button3Cmd="TC_1_node3.revisitNode();";
         button6 = "It sucks!";
		 button6Cmd="TC_1_node3.hateNode();";
         displayText = "Too bad you didn\'t like node 1.  This is node three.  What do you think?";
   };
};
//--- OBJECT WRITE END ---
function TC_1_node1::likeNode(%this)
{
	echo(" @@@ liked node 1");
}

function TC_1_node1::hateNode(%this)
{
	echo(" @@@ hated node 1");
}

function TC_1_node2::likeNode(%this)
{
	echo(" @@@ liked node 2");
}

function TC_1_node2::hateNode(%this)
{
	echo(" @@@ hated node 2");
}

function TC_1_node2::revisitNode(%this)
{
	echo(" @@@ node 2 looking at node 1 again.");
}

function TC_1_node3::likeNode(%this)
{
	echo(" @@@ liked node 3");
}

function TC_1_node3::hateNode(%this)
{
	echo(" @@@ hated node 3");
}

function TC_1_node3::revisitNode(%this)
{
	echo(" @@@ node 3 looking at node 1 again.");
}

