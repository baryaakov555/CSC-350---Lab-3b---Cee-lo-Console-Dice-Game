# 🎲 Cee-lo Console Dice Game

A simple console-based implementation of the classic dice gambling game **Cee-lo**, written in C#.

---

## 📜 Rules

1. The game uses **three dice**.
2. One player is the **Banker**, the other is the **Player**.
3. Both roll until they get a valid outcome for their turn.

---

### 🎯 Valid Outcomes
- **4-5-6** → Automatic **win**.  
- **1-2-3** → Automatic **loss**.  
- **Triples** → Higher triples beat lower triples. Triples beat any point.  
- **Point** → A pair plus another die; the odd die becomes the point. Higher point wins.  

---

### ⚖️ Comparison Rules
- The **Banker rolls first**.  
- If the Banker doesn’t instantly win or lose, they set a target (triples or point).  
- The **Player then tries to beat** the Banker’s result.

---

### 💰 Betting
- You wager an amount before rolling.  
- If you win, you gain that amount.  
- If you lose, you lose that amount.
