using UnityEngine;
using System.Collections;

public interface ISound {

    void PlaySound(int clip);

    void StopSound(int clip);
}
