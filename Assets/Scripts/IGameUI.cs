using System;
using System.Collections.Generic;

/// <summary>
/// interface for UI, called by gamesession to show UI and UI calls back when the player has acted
/// </summary>
public interface IGameUI
{
    void ShowMenu(Action onStartGame);
    void ShowQuestion(QuestionStep question, Action<int> onAnswerSelected);
    void ShowExplanation(string text, bool wasCorrect, Action onContinue);
}
