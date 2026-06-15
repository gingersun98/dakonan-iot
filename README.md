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
5. Tarik `Regular Hole` ke **Project → Prefabs** untuk menjadikan ke Prefab
6. Hapus `Regular Hole` di Scene.
  
### Langkah 2.3 — Memasang Lubang
1. Klik Kanan `Board` di Hierarchy → Create Empty
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
