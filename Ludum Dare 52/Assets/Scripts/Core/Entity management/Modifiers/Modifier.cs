using UnityEngine;

public class Modifier : MonoBehaviour
{
    #region ATTRIBUTES
    protected float _cooldownApplyModifier;                             //Cooldown for apply modifier
    private float _tempCooldownApplyModifier;                           //Temp variable for cooldown processing
    protected bool _isEnabled;                                          //If modifier is enabled or not
    protected bool _useCooldown;                                        //If modifier manage a cooldown
    protected float _lifeTime;                                          //Total life time
    protected float _currentLifeTime;                                   //Current life time
    protected float _destructionTime;                                   //Destruction time
    protected bool _destructionStarted;                                 //Destruction effect started
    #endregion

    #region PROTECTED UNITY METHODS
    /// <summary>
    /// METHOD : Call by inherit script to initialize script
    /// </summary>
    protected void ProtectedAwake()
    {
        _currentLifeTime = 0;
        _destructionStarted = false;
        _isEnabled = false;
        _useCooldown = false;
    }

    /// <summary>
    /// METHOD : Call by inherit script to manage cooldowns
    /// </summary>
    protected void ProtectedUpdate()
    {
        ManageCooldown();
        ManageTimeBeforeDestruction();
    }
    #endregion

    #region METHODS
    /// <summary>
    /// METHOD : Manage time before modifier destruction
    /// </summary>
    private void ManageTimeBeforeDestruction()
    {
        if ((_isEnabled) && (!_destructionStarted))
        {
            _currentLifeTime += Time.deltaTime;

            if (_currentLifeTime + _destructionTime >= _lifeTime)
            {
                _destructionStarted = true;
                StartDestruction(_lifeTime - _currentLifeTime);
            }
        }
    }

    /// <summary>
    /// METHOD : Start destruction process
    /// </summary>
    /// <param name="timeBeforeDestruction">Time before destruction</param>
    protected virtual void StartDestruction(float timeBeforeDestruction) { }

    /// <summary>
    /// METHOD : Manage modifier cooldowns
    /// </summary>
    private void ManageCooldown()
    {
        if ((_isEnabled) && (_useCooldown))
        {
            if (_tempCooldownApplyModifier < _cooldownApplyModifier)
            {
                _tempCooldownApplyModifier += Time.deltaTime;
            }
            else
            {
                _tempCooldownApplyModifier = 0.0f;

                ApplyEffect();
            }
        }
    }

    /// <summary>
    /// METHOD : Apply effect on all inherit scripts
    /// </summary>
    protected virtual void ApplyEffect() { }
    #endregion
}