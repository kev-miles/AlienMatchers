using UnityEngine;
using System.Collections;

public interface IPoolable {
	void OnAcquire();
	void OnRelease();
}
