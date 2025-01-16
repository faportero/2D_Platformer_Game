using InputFolder;
using UnityEngine;
using UnityEngine.UI;

namespace Character
{
    public class TutorialTaps: MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private Image[] _images;
        [SerializeField] private Animator _animator;

        private void Update()
        {
            if(!_images[0].gameObject.activeSelf && !_images[1].gameObject.activeSelf && !_images[2].gameObject.activeSelf && !_images[3].gameObject.activeSelf)
                Destroy(this);
            if (_animator.GetFloat("MovY") != 0)
            {
                if (_player.transform.rotation.eulerAngles.z > 0 && _player.transform.rotation.eulerAngles.z < 40)
                {
                    if (_images[0].gameObject.activeSelf)
                    {
                        _images[0].gameObject.SetActive(false);
                        return;
                    }
                }
                if (_player.transform.rotation.eulerAngles.z < 360 && _player.transform.rotation.eulerAngles.z > 320)
                {
                    if (_images[0].gameObject.activeSelf)
                    {
                        _images[0].gameObject.SetActive(false);
                        return;
                    }
                }
                if (_player.transform.rotation.eulerAngles.z > 120 && _player.transform.rotation.eulerAngles.z < 240)
                {
                    if (_images[1].gameObject.activeSelf)
                    {
                        _images[1].gameObject.SetActive(false);
                        return;
                    }
                }
            }
            if (_animator.GetFloat("MovX") != 0)
            {
                if ((_player.transform.rotation.eulerAngles.z > 50 && _player.transform.rotation.eulerAngles.z < 130))
                {
                    if (_images[2].gameObject.activeSelf)
                    {
                        _images[2].gameObject.SetActive(false);
                        return;
                    }
                }
                if ((_player.transform.rotation.eulerAngles.z > 220 && _player.transform.rotation.eulerAngles.z < 320))
                {
                    if (_images[3].gameObject.activeSelf)
                    {
                        _images[3].gameObject.SetActive(false);
                        return;
                    }
                }
            }
        }
    }
}