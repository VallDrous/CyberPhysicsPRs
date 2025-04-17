import numpy as np
from scipy.integrate import solve_ivp
import matplotlib.pyplot as plt
import unittest

SIGMA = 10.0
RHO = 28.0
BETA = 8.0 / 3.0

def lorenz(t, state):
    x, y, z = state
    dx_dt = SIGMA * (y - x)
    dy_dt = x * (RHO - z) - y
    dz_dt = x * y - BETA * z
    return [dx_dt, dy_dt, dz_dt]

def generate_trajectory(initial_state, t_span, t_eval):
    sol = solve_ivp(lorenz, t_span, initial_state, t_eval=t_eval, method='RK45')
    return sol.t, sol.y

def plot_lorenz_sensitivity():
    t_eval = np.linspace(0, 30, 10000)
    initial1 = [1.0, 1.0, 1.0]
    initial2 = [1.0001, 1.0, 1.0] 

    _, traj1 = generate_trajectory(initial1, (0, 30), t_eval)
    _, traj2 = generate_trajectory(initial2, (0, 30), t_eval)

    fig = plt.figure(figsize=(12, 6))
    ax = fig.add_subplot(121, projection='3d')
    ax.plot(traj1[0], traj1[1], traj1[2], color='blue', label='Початок: [1.0, 1.0, 1.0]')
    ax.plot(traj2[0], traj2[1], traj2[2], color='red', label='Початок: [1.0001, 1.0, 1.0]')
    ax.set_title("Атрактор Лоренца")
    ax.legend()

    ax2 = fig.add_subplot(122)
    distance = np.linalg.norm(traj1 - traj2, axis=0)
    ax2.plot(t_eval, distance)
    ax2.set_title("Відхилення між траєкторіями")
    ax2.set_xlabel("Час")
    ax2.set_ylabel("Євклідова відстань")

    plt.tight_layout()
    plt.show()

class TestLorenzModel(unittest.TestCase):
    def test_dimensions(self):
        t_eval = np.linspace(0, 10, 500)
        t, y = generate_trajectory([1.0, 1.0, 1.0], (0, 10), t_eval)
        self.assertEqual(y.shape, (3, len(t_eval)))

    def test_divergence(self):
        t_eval = np.linspace(0, 40, 10000) 
        _, traj1 = generate_trajectory([1.0, 1.0, 1.0], (0, 40), t_eval)
        _, traj2 = generate_trajectory([1.0001, 1.0, 1.0], (0, 40), t_eval)
        distance = np.linalg.norm(traj1 - traj2, axis=0)
        max_divergence = np.max(distance)
        self.assertGreater(max_divergence, 10.0)

if __name__ == "__main__":
    plot_lorenz_sensitivity()
    unittest.main(argv=[''], exit=False)