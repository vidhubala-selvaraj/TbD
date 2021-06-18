using UnityEngine;
using UnityEngine.SceneManagement;

public class Tower : MonoBehaviour
{
    private static readonly int MaxHealthPoint = 100;
    private static readonly int MinHealthPoint = 0;
    private int _healthPoint;
    private GameObject _tower;
    private GameObject[] _levels;
    private GameObject _leftTurret;
    private GameObject _rightTurret;
    private GameObject _passiveAttackSpawn;
    private BoxCollider2D _towerBoxCollider;
    private GameObject _topSection;

    // Start is called before the first frame update
    public void Start()
    {
        // initialize tower HP to the max value
        _healthPoint = MaxHealthPoint;

        // Find tower game object
        _tower = gameObject;// GameObject.Find("Tower");
        if (_tower == null) Debug.LogError("Can't find tower");

        // Find levels
        _levels = new GameObject[8];
        for (int i = 0; i < 8; ++i)
        {
            _levels[i] = _tower.transform.Find($"Level{i}").gameObject;
            if (_levels[i] == null) Debug.LogError($"Can't find Level{i}");
        }

        // Find turrets
        _leftTurret = _tower.transform.Find("LeftTurret").gameObject;
        if (_leftTurret == null) Debug.LogError("Can't find LeftTurret");
        _rightTurret = _tower.transform.Find("RightTurret").gameObject;
        if (_rightTurret == null) Debug.LogError("Can't find RightTurret");
        _topSection = _leftTurret;

        // Get towerBoxCollider
        _towerBoxCollider = gameObject.GetComponent<BoxCollider2D>();

        // Get passiveAttackSpawn
        _passiveAttackSpawn = _tower.transform.Find("PassiveAttackSpawn").gameObject;

        Debug.Log("Tower initialized");
    }

    // Update is called once per frame
    public void Update()
    {
        // check keyboard input. for testing
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            DamageTower(3);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ResetTower();
        }
    }

    /// <summary>
    /// Decrease tower HP. Call by the enemy units and enemy projectile objects
    /// </summary>
    /// <param name="damage"></param>
    public void DamageTower(int damage)
    {
        _healthPoint -= damage;
        UpdateTowerAppearance();
        UpdateTopSectionSpriteColor(damage);
        Debug.Log($"Tower took damage {damage}, HP becomes {_healthPoint}");

        if (_healthPoint <= MinHealthPoint)
        {
            GameOver();
            Debug.Log("GameOver");
        }
    }

    /// <summary>
    /// Reset tower HP. For testing
    /// </summary>
    public void ResetTower()
    {
        _healthPoint = MaxHealthPoint;
        ResetTowerSpriteColor();
        UpdateTowerAppearance();
        Debug.Log($"Reset Tower, HP becomes {_healthPoint}");
    }

    /// <summary>
    /// Update the appearance of the tower, corresponding to current HP value
    /// </summary>
    private void UpdateTowerAppearance()
    {
        // set visibility of each level
        if (_healthPoint > 90)
        {
            _leftTurret.SetActive(true);
            _rightTurret.SetActive(true);
            _topSection = _leftTurret;
            foreach (GameObject level in _levels)
            {
                level.SetActive(true);
            }
            UpdateTowerColliderShape(9, -3f);
            _passiveAttackSpawn.SendMessage("EnableArrowSpawn");
            _passiveAttackSpawn.SendMessage("EnableBombSpawn");
        }
        if (_healthPoint <= 90)
        {
            _leftTurret.SetActive(false);
            _topSection = _rightTurret;
            UpdateTowerColliderShape(9, -3f);
        }
        if (_healthPoint <= 80)
        {
            _rightTurret.SetActive(false);
            _topSection = _levels[7];
            UpdateTowerColliderShape(8, -3.5f);
        }
        if (_healthPoint <= 70)
        {
            _levels[7].SetActive(false);
            _topSection = _levels[6];
            UpdateTowerColliderShape(7, -4f);
            _passiveAttackSpawn.SendMessage("DisableArrowSpawn");
        }
        if (_healthPoint <= 60)
        {
            _levels[6].SetActive(false);
            _topSection = _levels[5];
            UpdateTowerColliderShape(6, -4.5f);
        }
        if (_healthPoint <= 50)
        {
            _levels[5].SetActive(false);
            _topSection = _levels[4];
            UpdateTowerColliderShape(5, -5f);
        }
        if (_healthPoint <= 40)
        {
            _levels[4].SetActive(false);
            _topSection = _levels[3];
            UpdateTowerColliderShape(4, -5.5f);
            _passiveAttackSpawn.SendMessage("DisableBombSpawn");
        }
        if (_healthPoint <= 30)
        {
            _levels[3].SetActive(false);
            _topSection = _levels[2];
            UpdateTowerColliderShape(3, -6f);
        }
        if (_healthPoint <= 20)
        {
            _levels[2].SetActive(false);
            _topSection = _levels[1];
            UpdateTowerColliderShape(2, -6.5f);
        }
        if (_healthPoint <= 10)
        {
            _levels[1].SetActive(false);
            _topSection = _levels[0];
            UpdateTowerColliderShape(1, -7f);
        }
        if (_healthPoint <= 0)
        {
            _levels[0].SetActive(false);
            UpdateTowerColliderShape(0, -7.5f);
            Destroy(this.gameObject);
        }
    }

    private void UpdateTowerColliderShape(int sizeY, float offsetY)
    {
        _towerBoxCollider.size = new Vector2(3, sizeY);
        _towerBoxCollider.offset = new Vector2(-1, offsetY);
    }

    private void UpdateTopSectionSpriteColor(int damage)
    {
        Color newColor = _topSection.GetComponent<SpriteRenderer>().color;
        float colorOffset = 0.05f * damage;
        newColor.r -= colorOffset;
        newColor.g -= colorOffset;
        newColor.b -= colorOffset;
        _topSection.GetComponent<SpriteRenderer>().color = newColor;
    }

    private void ResetTowerSpriteColor()
    {
        for (int i = 0; i < 8; ++i)
        {
            _levels[i].GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f);
        }
        _leftTurret.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f);
        _rightTurret.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f);
    }

    public int GetHealthPoint()
    {
        return _healthPoint;
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOverScreen");
    }

}
