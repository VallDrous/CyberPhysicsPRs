import numpy as np
import matplotlib.pyplot as plt
from scipy.integrate import solve_ivp

a, b, c = 0.2, 0.2, 5.7

def rossler(t, state):
    x, y, z = state
    dx = -y - z
    dy = x + a * y
    dz = b + z * (x - c)
    return [dx, dy, dz]

initial1 = [1.0, 1.0, 1.0]
initial2 = [1.0001, 1.0, 1.0]

t_eval = np.linspace(0, 200, 10000)

_, traj1 = solve_ivp(rossler, (0, 200), initial1, t_eval=t_eval).t, solve_ivp(rossler, (0, 200), initial1, t_eval=t_eval).y
_, traj2 = solve_ivp(rossler, (0, 200), initial2, t_eval=t_eval).t, solve_ivp(rossler, (0, 200), initial2, t_eval=t_eval).y

fig = plt.figure(figsize=(12, 6))

ax1 = fig.add_subplot(121, projection='3d')
ax1.plot(traj1[0], traj1[1], traj1[2], color='blue', label='[1.0, 1.0, 1.0]')
ax1.plot(traj2[0], traj2[1], traj2[2], color='red', label='[1.0001, 1.0, 1.0]')
ax1.set_title("Атрактор Росслера")
ax1.legend()

distance = np.linalg.norm(traj1 - traj2, axis=0)
ax2 = fig.add_subplot(122)
ax2.plot(t_eval, distance)
ax2.set_title("Відхилення між траєкторіями")
ax2.set_xlabel("Час")
ax2.set_ylabel("Євклідова відстань")

plt.tight_layout()
plt.show()