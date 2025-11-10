/*Sustainability js start here*/

document.addEventListener("DOMContentLoaded", function () {
  gsap.registerPlugin(ScrollTrigger);
  let width = window.innerWidth;
  if(width >= 1001){


gsap.from(".sustain_text", {
    y: 100,
    opacity: 0,
    duration: 1,
    scrollTrigger: {
        trigger: ".home_sustainbility",
        start: "top 80%",
        toggleActions: "play none none reverse"
    }
});

gsap.from(".icon_sustain", {
      opacity: 0,
      x: 100,
      rotate: 10,
      y: 50,
      duration: 1.2,
      ease: "power2.out",
      scrollTrigger: {
          trigger: ".home_sustainbility",
          start: "top 70%",
          toggleActions: "play none none reverse"
      }
  });
      gsap.to(".c-shape-reveal", {
          "--reveal-clip": "circle(75% at 50% 50%)",
          duration: 1.5,
          ease: "power2.out",
          scrollTrigger: {
              trigger: ".home_sustainbility",
              start: "top 75%",
              toggleActions: "play none none reverse"
          }
      });



      gsap.registerPlugin(ScrollTrigger);

      // Fade and slide effect for .left-slide and .right-container
      // After GSAP animation finishes
      gsap.from(".left-slide > *, .right-container > div", {
          scrollTrigger: {
              trigger: ".project_slider",
              start: "top center",
              toggleActions: "play none none none",
          },
          autoAlpha: 0,
          y: 30,
          duration: 0.5,
          stagger: 0.3,
          ease: "power3.out",
          onComplete: () => {
              // After animation, reset styles so next/prev slides don’t stay hidden
              document.querySelectorAll(".left-slide > *, .right-container > div").forEach(el => {
                  el.style.opacity = "1";
                  el.style.transform = "none";
                  el.style.visibility = "visible";
              });
          }
      });
  
      // Scroll-linked movement for .icon_project
      gsap.to(".icon_project", {
          y: 700, // Move down by 300px
          ease: "none",
          scrollTrigger: {
              trigger: ".icon_project",
              start: "top top",
              end: "bottom+=100 top",
              scrub: true
          }
      });
    }
});
    
      /*Sustainability js end here*/

/*happening js start here*/

      document.addEventListener("DOMContentLoaded", function () {
        const items = document.querySelectorAll(".skipper_news .item");

        const observer = new IntersectionObserver((entries) => {
            entries.forEach((entry, index) => {
                if (entry.isIntersecting) {
                    const item = entry.target;
                    // Animate item block
                    setTimeout(() => {
                        entry.target.classList.add("visible");
                    }, index * 200); // 200ms delay between items
                 
                    // After item animation, animate content inside with delay
                    const content = item.querySelector(".home_news_detail");
                    if (content) {
                        setTimeout(() => {
                            content.classList.add("visible");
                        }, 400); // delay after item appears
                    }

                    observer.unobserve(item);
                }
            });
        }, {
            threshold: 0.3
        });

        items.forEach(item => observer.observe(item));
    });

    setTimeout(() => {
      var swiper = new Swiper(".mySwiper", {
          loop: true,
          navigation: {
              nextEl: ".swiper-button-next",
              prevEl: ".swiper-button-prev",
          },
          spaceBetween: 30,
      });
  }, 1500); // after project_slider fade-in

  /*happening  js end here*/

  