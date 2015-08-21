//-----------------------------------------------------------------------------
// Clockwork
// 5 Dec 2012
// Conversation gui scripts
//-----------------------------------------------------------------------------
function ConversationGui::onWake(%this)
{
	%this.updateIconDisplay("CW_standby", 30);
}

function ConversationGui::onSleep(%this)
{
	CW_IconDisplay.stop();
}

function ConversationGui::onDialogPush(%this)
{
	%this.startConversation(%this);
}

/// <summary>
/// This begins the conversation currently assigned to the ConversationGui
/// object.  This must be done before pushing the gui.
function ConversationGui::startConversation(%this)
{
	// Load conversation data
	moveMap.pop();
	%this.conversation = TC_1;
	%node = %this.conversation.startConversation();
	%this.displayNode(%node);
}

function ConversationGui::displayNode(%this, %node)
{
	// Update the gui with the current conversation node information
	if ( !isObject(%node) )
	{
		echo(" @@@ invalid conversation node!");
		return;
	}
	echo(" @@@ displaying node " @ %node.getName());
	CW_LowerTextDisplay.setText(%node.displayText);

	if ( %node.button1 !$= "" )
	{
		CW_Button1.setText(%node.button1);
		CW_Button1.setVisible(true);
	}
	else
		CW_Button1.setVisible(false);
	CW_Button1.nextNode = %node.button1next;
	CW_Button1.cmd = %node.button1Cmd;
	if ( %node.button2 !$= "" )
	{
		CW_Button2.setText(%node.button2);
		CW_Button2.setVisible(true);
	}
	else
		CW_Button2.setVisible(false);
	CW_Button2.nextNode = %node.button2next;
	CW_Button2.cmd = %node.button2Cmd;
	if ( %node.button3 !$= "" )
	{
		CW_Button3.setText(%node.button3);
		CW_Button3.setVisible(true);
	}
	else
		CW_Button3.setVisible(false);
	CW_Button3.nextNode = %node.button3next;
	CW_Button3.cmd = %node.button3Cmd;
	if ( %node.button4 !$= "" )
	{
		CW_Button4.setText(%node.button4);
		CW_Button4.setVisible(true);
	}
	else
		CW_Button4.setVisible(false);
	CW_Button4.nextNode = %node.button4next;
	CW_Button4.cmd = %node.button4Cmd;
	if ( %node.button5 !$= "" )
	{
		CW_Button5.setText(%node.button5);
		CW_Button5.setVisible(true);
	}
	else
		CW_Button5.setVisible(false);
	CW_Button5.nextNode = %node.button5next;
	CW_Button5.cmd = %node.button5Cmd;
	if ( %node.button6 !$= "" )
	{
		CW_Button6.setText(%node.button6);
		CW_Button6.setVisible(true);
	}
	else
		CW_Button6.setVisible(false);
	CW_Button6.nextNode = %node.button6next;
	CW_Button6.cmd = %node.button6Cmd;
	if ( %node.button7 !$= "" )
	{
		CW_Button7.setText(%node.button7);
		CW_Button7.setVisible(true);
	}
	else
		CW_Button7.setVisible(false);
	CW_Button7.nextNode = %node.button7next;
	CW_Button7.cmd = %node.button7Cmd;
	if ( %node.button8 !$= "" )
	{
		CW_Button8.setText(%node.button8);
		CW_Button8.setVisible(true);
	}
	else
		CW_Button8.setVisible(false);
	CW_Button8.nextNode = %node.button8next;
	CW_Button8.cmd = %node.button8Cmd;
}

function ConversationGui::exitConversation(%this)
{
	// Handle conversation exit.  If we're exiting early, allow for exit callback.
	if ( %this.conversation.hasExitScript )
		%this.conversation.onConversationEnd();

	Canvas.popDialog(%this);
	moveMap.push();
}

function ConversationGui::onConversationEnd(%this)
{
	// This is the last thing that will occur before the conversation terminates.
	// Faction values, start attack, update inventory or whatever else needs to be done
	// should happen here.
	// We're simply calling the conversation's own exit script, so if anything needs to
	// happen it needs to be there.
	%this.conversation.onExit();
}

function ConversationGui::updateIconDisplay(%this, %image, %frameCount)
{
	CW_IconDisplay.startTick(%image, %frameCount, 1000 / %frameCount);  // default for now
}

function CW_IconDisplay::startTick(%this,%imageName, %frameCount, %framerate)
{
	%this.image = %imageName;
	%this.frameCount = %frameCount;
	%this.currentFrame = 0;
	%this.updateTime = %framerate;
	%this.updateTick();
}

function CW_IconDisplay::updateTick(%this)
{
	if (%this.currentFrame > %this.frameCount)
		%this.currentFrame = 0;
	if (%this.currentFrame < 10)
		%frame = "000" @ %this.currentFrame;
	else
		%frame = "00" @ %this.currentFrame;
	%bitmap = "art/gui/ConversationWheel/" @ %this.image @ %frame;
	%this.setBitmap(%bitmap);
	%this.currentFrame++;
	%this.updateSchedule = %this.schedule(%this.updateTime, updateTick);
}

function CW_IconDisplay::stop(%this)
{
	if (%this.updateSchedule !$= "")
		cancel(%this.updateSchedule);
	%this.updateSchedule = "";
}

function CW_Button::onMouseUp(%this)
{
	// Handle the conversation button clicks here.  These will have to be tied to 
	// data from the current conversation node to know if there are any actions
	// to take, which node to move to, etc.
	eval(%this.cmd);
	if ( %this.nextNode !$= "" )
		ConversationGui.displayNode(%this.nextNode);
	else
		ConversationGui.exitConversation();
}

function CW_Button::onMouseEnter(%this)
{
	%index = strreplace(%this.getName(), "CW_Button", "");
	//CW_LowerTextDisplay.setText(" @@@ button "@%index@" onMouseEnter");
}

function CW_Button::onMouseLeave(%this)
{
	%index = strreplace(%this.getName(), "CW_Button", "");
	//CW_LowerTextDisplay.setText(" @@@ button "@%index@" onMouseLeave");
}

function CW_Button::onMouseDown(%this)
{
	%index = strreplace(%this.getName(), "CW_Button", "");
	//CW_LowerTextDisplay.setText(" @@@ button "@%index@" onMouseDown");
}

