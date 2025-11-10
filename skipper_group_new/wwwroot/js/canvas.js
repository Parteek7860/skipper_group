
  document.addEventListener("DOMContentLoaded", function () {
    const canvas = document.getElementById('donutChart');
    const tooltip = document.getElementById('tooltip');
    const ctx = canvas.getContext('2d');

    const data = [
      { value: 0.68, color: "#0c3c60", label: "Engineering Products" },
      { value: 0.18, color: "#6e90a6", label: "Infrastructure Projects" },
      { value: 0.14, color: "#e68527", label: "Polymer Products" }
    ];

    const centerX = canvas.width / 2;
    const centerY = canvas.height / 2;
    const radius = 150;
    const thickness = 35;

    let animationProgress = 0;
    const animationSpeed = 0.02;
    let angles = [];

    function drawDonut(progress) {
      ctx.clearRect(0, 0, canvas.width, canvas.height);
      let startAngle = -Math.PI / 2;
      angles = [];

      data.forEach(segment => {
        const angle = segment.value * 2 * Math.PI * Math.min(progress, 1);
        const endAngle = startAngle + angle;
        ctx.beginPath();
        ctx.arc(centerX, centerY, radius, startAngle, endAngle);
        ctx.lineWidth = thickness;
        ctx.strokeStyle = segment.color;
        ctx.stroke();
        angles.push({ startAngle, endAngle, ...segment });
        startAngle = endAngle;
      });
    }

    function animate() {
      animationProgress += animationSpeed;
      drawDonut(animationProgress);
      if (animationProgress < 1) {
        requestAnimationFrame(animate);
      }
    }

    function animateDonutChart() {
      animate();
    }

    const observer = new IntersectionObserver((entries, obs) => {
      entries.forEach(entry => {
        if (entry.isIntersecting) {
          animateDonutChart();
          obs.disconnect();
        }
      });
    }, { threshold: 0.5 });

    observer.observe(document.querySelector('.investor_two'));

    canvas.addEventListener('mousemove', (e) => {
      const rect = canvas.getBoundingClientRect();
      const mouseX = e.clientX - rect.left;
      const mouseY = e.clientY - rect.top;

      const dx = mouseX - centerX;
      const dy = mouseY - centerY;
      const distance = Math.sqrt(dx * dx + dy * dy);

      if (distance >= radius - thickness && distance <= radius) {
        let angle = Math.atan2(dy, dx);
        if (angle < -Math.PI / 2) angle += 2 * Math.PI;

        for (let segment of angles) {
          if (angle >= segment.startAngle && angle <= segment.endAngle) {
            tooltip.style.display = 'block';
            tooltip.style.left = `${mouseX + 20}px`;
            tooltip.style.top = `${mouseY + 20}px`;
            tooltip.innerHTML = `<strong>${segment.label}</strong>: ${(segment.value * 100).toFixed(0)}%`;
            return;
          }
        }
      }

      tooltip.style.display = 'none';
    });

    canvas.addEventListener('mouseleave', () => {
      tooltip.style.display = 'none';
    });
  });

