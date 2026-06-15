# 🎮 TUTORIAL GAME CONGKLAK - UNITY
### Versi 1.0 - Konversi Berat ke Credit, Bermain Congklak, DLL.
---

## 🗺️ ALUR GAME
```
┌─────────────────────────┐
│       Menu Utama        │
└───────────┬─────────────┘
            │
    ┌───────┼───────────────┬───────────────┐
    │       │               │               │
    ▼       ▼               ▼               ▼
   MAIN  PENGATURAN       CREDIT          KELUAR
    │       │               │               │
    │       │               │               ▼
    │       │               │        Keluar Aplikasi
    │       │               │
    │       │               ▼
    │       │        Scan Kode QR
    │       │               │
    │       │               ▼
    │       │   Sambungkan Aplikasi ke Timbangan
    │       │               │
    │       │               ▼
    │       │    Masukan Sampah ke Timbangan
    │       │               │
    │       │               ▼
    │       │      Hitung Berat Sampah
    │       │               │
    │       │               ▼
    │       │      Setiap 100g = 1 Credit
    │       │               │
    │       │               ▼
    │       │          Tukar Credits
    │       │               │
    │       │               ▼
    │       │       Kembali ke Menu Utama
    │       │
    │       ▼
    │   Ganti Volume Musik
    │           │
    │           ▼
    │    Ganti Volume SFX
    │           │
    │           ▼
    │    Kembali ke Menu Utama
    │
    ▼
Pilih Mode Game [CONGKLAK, GASING, KETAPEL, KELERENG]
    │
    ▼
Cek Credits (Apakah Punya 5?)
    │
 ┌──┴─────┐
 │        │
 ▼        ▼
Tidak     Ya
 │        │
 ▼        ▼
Menu    Pakai
Utama  5 Credits
          │
          ▼
┌─────────────────────────┐
│    MULAI CONGKLAK       │
└─────────┬───────────────┘
 Batu Gunting Kertas
(Menentukan Giliran Pertama)
          │
          ▼
     Mulai Permainan
          │
          ▼
 Pemain Mengambil Batu
     Ke Rumah Sendiri
          │
          ▼
 Apakah semua batu sudah masuk ke rumah?
      ┌───┴───┐
      │       │
      ▼       ▼
    Tidak     Ya
      │       │
      ▼       ▼
 Lanjut     Hitung Total
 Bermain    Batu dalam Rumah
                │
                ▼
          Bandingkan Jumlah
                │
                ▼
         Pilih Pemenang
                │
                ▼
           Main Lagi?
            ┌──┴──┐
            │     │
            Ya    Tidak
            │     │
            ▼     ▼
    Mulai Match    Menu
     Dari Awal     Utama
```

---

## 📁 TUTORIAL 1 - PERSIAPAN PROJEK

### Langkah 1.1 — Buat Project Baru
1. Buka **Unity Hub → New Project**
2. Pilih template: **Universal 2D**
3. Nama: `Congklak-Dakon`
4. Klik **Create Project**

### Langkah 1.2 — Buat Struktur Folder
Klik kanan di panel **Project → Create → Folder**:
```
Assets/
├── Scripts/
├── Scenes/
├── Sprites/
└── Prefabs/
```

### Langkah 1.3 — Buat 2 Scene
**File → New Scene → Save** sebanyak 3x:

| Nama Scene | Simpan di | Tujuan |
|------------|-----------|--------|
| `MainMenu` | Assets/Scenes/ | Memilih mode game, mengkonversi kredit, dll. |
| `Gameplay` | Assets/Scenes/ | Menyimpan permainan yang akan dimainkan. |

### Langkah 1.4 — Daftarkan ke Build Settings
1. **File → Build Settings**
2. Drag semua scene ke kotak **Scenes In Build**
3. Urutan wajib:

| Index | Scene |
|-------|-------|
| 0 | MainMenu |
| 1 | Gameplay |

---

## 🗺️ TUTORIAL 2 - PEMBUATAN TAMPILAN CONGKLAK

Buka scene "Gameplay" untuk memulai tutorial ini. Kita membuat tampilan sederhana untuk permainan `Congklak`.

### Langkah 2.1 — Buat Papan Gameplay
1. Hierarchy → Klik Kanan → **2D Object → Sprites → Square**
2. Rename: `Board`
3. **Transform:**
   - Position: (0, 0, 0)
   - Scale: (1, 1, 1)
4. **Sprite Renderer**
   - Color: (233, 175, 85, 255) → Bebas warna apa, sesuaikan dengan selera.
   - Draw Mode : Sliced
   - Size : (2.11, 8.25) → Tidak harus sama persis, sesuaikan dengan ukuran layar.
   - Order In Layer : -1

### Langkah 2.2 — Buat Satu Lubang
1. Hierarchy → Klik Kanan → **2D Object → Sprites → Square**
2. Rename: `Regular Hole`
3. **Sprite Renderer**
   - Color: `(0, 120, 202, 255)` untuk warna pemain, `(156, 0, 21, 255)` untuk warna musuh. → Pilih salah satu terlebih dahulu, kita akan mengganti warnanya disaat Langkah 2.3
   - Draw Mode : Sliced
   - Size : (0.71, 0.63) → Tidak harus sama persis, sesuaikan dengan ukuran `Board`.
   - Order In Layer : 0
4. Tambahkan 6 komponen ini di dalam `Regular Hole` :

| Nama Komponen | Offset | Size/Radius | IsTrigger |
|---------------|--------|-------------|-----------|
| `CircleCollider2D` | (0, 0) | 0.01 | ❌ |
| `BoxCollider2D` | (-0.35, 0)* | (0.05, 0.61)* | ❌ |
| `BoxCollider2D` | (0.35, 0)* | (0.05, 0.61)* | ❌ |
| `BoxCollider2D` | (0, 0.32)* | (0.76, 0.05)* | ❌ |
| `BoxCollider2D` | (0, -0.31)* | (0.76, 0.05)* | ❌ |
| `BoxCollider2D` | (0, 0) | (0.68, 0.68) | ✅ |

*Tidak harus sama persis, asalkan bisa membentuk dinding yang menutupi `Regular Hole`

5. Buat script baru di dalam folder **Assets → Scripts** dengan nama `BallContainer.cs`.

`BallContainer.cs` berguna untuk memainkan dan menyimpan bola/batu yang akan digunakan untuk bermain congklak.
```csharp
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallContainer : MonoBehaviour
{
    #region State

    [Header("State")]
    public bool isPlayerSide;
    public bool isSelected;

    #endregion

    #region UI

    [Header("UI")]
    public GameObject playerText;
    public GameObject enemyText;

    public TextMeshPro[] displayText;

    public GameObject selectedIndicator;

    #endregion

    #region Ball Storage

    [Header("Ball Storage")]
    List<GameObject> ballInside = new List<GameObject>();

    #endregion

    #region Animation

    [Header("Animation")]
    Coroutine jitterCoroutine;

    float maxJitterAngle = 45f;
    float recoverySpeed = 10f;

    Quaternion originalRotation;

    #endregion

    private void Start()
    {
        originalRotation = displayText[0].transform.localRotation;
    }

    private void Update()
    {
        selectedIndicator.SetActive(isSelected && isPlayerSide);
    }

    public void TriggerJitter()
    {
        // If it's already jittering, stop it so we don't break the loop
        if (jitterCoroutine != null)
        {
            StopCoroutine(jitterCoroutine);
        }

        jitterCoroutine = StartCoroutine(DoJitter());
    }

    private IEnumerator DoJitter()
    {
        // 1. Pick a random direction: left (negative) or right (positive)
        // We use a range that excludes 0 so it always snaps noticeably
        Transform whatToMove = isPlayerSide ? displayText[1].transform : displayText[0].transform;
        float randomSign = Random.value > 0.5f ? 1f : -1f;
        float randomAngle = Random.Range(maxJitterAngle * 0.5f, maxJitterAngle) * randomSign;

        // 2. Snap instantly to the jittered rotation
        // Note: For 2D / Top-down, use Vector3.forward (Z-axis). For 3D characters, use Vector3.up (Y-axis).
        Quaternion jitteredRotation = originalRotation * Quaternion.AngleAxis(randomAngle, Vector3.forward);
        whatToMove.localRotation = jitteredRotation;

        // 3. Smoothly rotate back to the original rotation
        while (Quaternion.Angle(whatToMove.localRotation, originalRotation) > 0.1f)
        {
            whatToMove.localRotation = Quaternion.Slerp(
                whatToMove.localRotation,
                originalRotation,
                Time.deltaTime * recoverySpeed
            );

            yield return null; // Wait for the next frame
        }

        // Snap precisely to the end to prevent micro-movements
        whatToMove.localRotation = originalRotation;
    }

    public void InsertBall(GameObject ball)
    {
        ball.transform.SetParent(transform, true);
        ballInside.Add(ball);
        UpdateUI();
        TriggerJitter();
    }

    public GameObject[] TakeAllBalls()
    {
        GameObject[] newArray = ballInside.ToArray();
        ballInside.Clear();
        UpdateUI();
        return newArray;
    }

    public int CheckBallCount()
    {
        return ballInside.Count;
    }

    public void OnClicked()
    {
        if (!isPlayerSide || CheckBallCount() <= 0)
            return;
        SoundManager.instance.Play("click");
        if (!isSelected)
        {
            GameManager.Instance.DeselectAll();
            isSelected = true;
        } else
        {
            StartCoroutine(GameManager.Instance.StartPlayerTurn(this));
        }
    }

    public void UpdateUI()
    {
        if (CheckBallCount() <= 0)
        {
            playerText.gameObject.SetActive(false);
            enemyText.gameObject.SetActive(false);
            return;
        }
        playerText.gameObject.SetActive(isPlayerSide);
        enemyText.gameObject.SetActive(!isPlayerSide);

        foreach(TextMeshPro text in displayText)
        {
            text.text = CheckBallCount().ToString();
        }
    }
}
```
6. Tambahkan script ini ke dalam `Regular Hole`.
7. Buat indikator jumlah bola dalam lubang menggunakan `TextMeshPro`. Klik kanan `Regular Hole` di **Hierarchy → 2D Object → Sprites → Square**, lalu rename menjadi `Player Indicator`. Pasang di kanannya `Regular Hole`.
8. Klik kanan `Player Indicator` di **Hierarchy → Create Empty**, lalu rename menjadi `Player Display Text`. Rapikan sesuai keinginanmu.
9. Duplikat `Player Indicator`, rename menjadi `Enemy Indicator`. Pasang di kirinya `Regular Hole`.
10. Di `BallContainer.cs` dalam `Regular Hole` :
- Tarik `Player Indicator` ke variable **Player Indicator**.
- Tarik `Enemy Indicator` ke variable **Enemy Indicator**.
- Tarik kedua `Player Display Text` ke variable **Display Text**.
11. Buat indicator ketika pemain memilih lubang ini, seperti :
- Klik kanan `Regular Hole` di **Hierarchy → Create Empty**, rename menjadi `Selected Indicator`.
- Taruh di posisi (0, 0, 0).
- Klik kanan `Selected Indicator` di **Hierarchy → Create Empty**, rename menjadi `Indicator Text`.
- Tambah `TextMeshPro` di dalam `Indicator Text`. Tulis isinya dengan **"Main?"**. Ubah **Order In Layer** dalam `TextMeshPro` menjadi 2.
- Tarik `Selected Indicator` ke variable `Selected Indicator` di `BallContainer.cs`.
12. Tarik `Regular Hole` ke **Project → Prefabs** untuk menjadikan ke Prefab.
13. Hapus `Regular Hole` di Scene.
  
### Langkah 2.3 — Memasang Lubang
1. Klik kanan `Board` di Hierarchy → Create Empty
2. Rename : `Player Grid Holes`
3. **Transform:**
   - Position: (0.47, 0, 0)
   - Scale: (1, 1, 1)
4. Add Component → New script → `SpriteGridLayout` :
`SpriteGridLayout.cs` digunakan untuk menyusun objek dalam scene diluar `Canvas`, seperti `GridLayoutGroup` tapi untuk `SpriteRenderer`.
```csharp
using UnityEngine;

[ExecuteAlways]
public class SpriteGridLayout : MonoBehaviour
{
    public Vector2 cellSize = new Vector2(1f, 1f);
    public Vector2 spacing = new Vector2(0.1f, 0.1f);

    public int columns = 4;

    public bool centerGrid = false;

    [ContextMenu("Refresh Grid")]
    public void RefreshGrid()
    {
        int childCount = transform.childCount;

        int rows = Mathf.CeilToInt((float)childCount / columns);

        Vector2 offset = Vector2.zero;

        if (centerGrid)
        {
            float width = (columns - 1) * (cellSize.x + spacing.x);
            float height = (rows - 1) * (cellSize.y + spacing.y);

            offset = new Vector2(-width / 2f, height / 2f);
        }

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);

            int row = i / columns;
            int col = i % columns;

            Vector3 pos = new Vector3(
                col * (cellSize.x + spacing.x),
                -row * (cellSize.y + spacing.y),
                0f
            );

            child.localPosition = pos + (Vector3)offset;
        }
    }

    private void OnValidate()
    {
        RefreshGrid();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            RefreshGrid();
#endif
    }
}
```
5. Jangan lupa untuk memindah file `SpriteGridLayout.cs` di folder Scripts.
6. Ambil prefab `Regular Hole` dan tarik ke objek `Player Grid Holes` di Hierarchy. Lakukan ini sebanyak 7 kali.
7. Ganti semua warna `Regular Holes` di dalam `Player Grid Holes` menjadi warna pemain, yaitu `(0, 120, 202, 255)`.
8. Nyalakan `IsPlayerSide` dalam `BallContainer.cs` di masing-masing `Regular Holes` yang ada di dalam `Player Grid Holes`.
9. Duplikat `Player Grid Holes` lalu rename ke `Enemy Grid Holes`.
10. **`Enemy Grid Holes` Transform:**
   - Position: (-0.47, 0, 0)
   - Scale: (1, 1, 1)
11. Ganti semua warna `Regular Holes` di dalam `Enemy Grid Holes` menjadi warna musuh, yaitu `(156, 0, 21, 255)`.
12. Matikan `IsPlayerSide` dalam `BallContainer.cs` di masing-masing `Regular Holes` yang ada di dalam `Enemy Grid Holes`.

### Langkah 2.4 — Membuat Rumah Pemain dan Musuh
1. Tarik prefab `Regular Hole` dari foldernya ke dalam `Board`. Rename menjadi `Player's Base`.
2. **Transform** :
   - Position: (0, -3.38, 0)
3. **Sprite Renderer** :
   - Color: `(0, 120, 202, 255)`
   - Size: `(1.74, 1.03)`
4. **BallContainer.cs** :
   - IsPlayerSide: `false`
   - PlayerIndicator: `null`
   - EnemyIndicator: `null`
5. Ubah `BoxCollider2D` sehingga sesuai dengan ukuran baru. Untuk `BoxCollider2D` dengan **IsTrigger** yang nyala, hapus. Karena tidak akan digunakan.

| Nama Komponen | Offset | Size | IsTrigger |
|---------------|--------|---------|-----------|
| `BoxCollider2D` | (-0.85, 0)* | (0.05, 1.03)* | ❌ |
| `BoxCollider2D` | (0.85, 0)* | (0.05, 1.03)* | ❌ |
| `BoxCollider2D` | (0, 0.5)* | (1.74, 0.05)* | ❌ |
| `BoxCollider2D` | (0, -0.5)* | (1.74, 0.05)* | ❌ |
| `BoxCollider2D` | HAPUS | HAPUS | ✅ |

*Tidak harus sama persis, asalkan bisa membentuk dinding yang menutupi `Regular Hole`
6. Duplikat `Player's Base` dan rename menjadi `Enemy's Base`.
7. **Transform** :
   - Position: (0, 3.38, 0)
8. **Sprite Renderer** :
   - Color: `(156, 0, 21, 255)`
