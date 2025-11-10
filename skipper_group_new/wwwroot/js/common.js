AOS.init({
  easing: 'ease-out-back',
  disable: 'mobile',
  duration: 1000,
  once: true,
});
$(window).on('scroll', function () {
    // console.log(window.scrollY)
    if (window.scrollY > 150) {
        $('header').addClass('header-sticky')
    } else {
        $('header').removeClass('header-sticky')
    }
});
gsap.registerPlugin(ScrollTrigger);  
$(document).ready(function(){
  let width = window.innerWidth;
  if(width >= 992){
    gsap.utils.toArray(".image-container").forEach(function(container) {
      let image = container.querySelector("img");
    
      let tl = gsap.timeline({
          scrollTrigger: {
            trigger: container,
            scrub: true,
            pin: false,
          },
        }); 
        tl.from(image, {
          yPercent: -30,
          ease: "none",
        }).to(image, {
          yPercent: 30,
          ease: "none",
        }); 
    });
  }
})


let revealContainers = document.querySelectorAll(".reveal");

function wipeOn() {
    var wipers = document.querySelectorAll('.image');
    wipers.forEach(function(wipers) {
        var rect = wipers.getBoundingClientRect();
        if (rect.top < (window.innerHeight - 200 || document.documentElement.clientHeight)) {
            wipers.classList.add('reveal-image');
        }
        // else{
        //     wipers.classList.remove('reveal-image');
        // }
    });


    var wipers1 = document.querySelectorAll('.image2');
    wipers1.forEach(function(wipers1) {
        var rect1 = wipers1.getBoundingClientRect();
        if (rect1.top < (window.innerHeight - 200 || document.documentElement.clientHeight)) {
            wipers1.classList.add('reveal-image2');
        }
        // else{
    
        // else{
        //     wipers1.classList.remove('reveal-image2');
        // }
    });
    var wipers2 = document.querySelectorAll('.image3');
    wipers2.forEach(function(wipers2) {
        var rect2 = wipers2.getBoundingClientRect();
        if (rect2.top < (window.innerHeight - 200 || document.documentElement.clientHeight)) {
            wipers2.classList.add('reveal-image3');
        }
      });
    var wipers3 = document.querySelectorAll('.image4');
    wipers3.forEach(function(wipers3) {
        var rect3 = wipers3.getBoundingClientRect();
        if (rect3.top < (window.innerHeight - 100 || document.documentElement.clientHeight)) {
            wipers3.classList.add('reveal-image4');
        }
    });

};
    window.addEventListener('load', wipeOn);
    window.addEventListener('scroll', wipeOn);
    window.addEventListener('resize', wipeOn);



    





// Mega Menu JS Start//
jQuery(document).ready(function () {
    const menuItemSelector = '.menu-item.menudrop-item';
    const dropdownSelector = '.mega_dropdown';
    const subMenuLinkSelector = '.dropmenu';
  
    // Show mega dropdown
    jQuery(menuItemSelector).mouseenter(function () {
      const $this = jQuery(this);
  
      jQuery(menuItemSelector).removeClass('active');
      jQuery(dropdownSelector).removeClass('show');
  
      $this.addClass('active').children(dropdownSelector).addClass('show');
  
      // Default to first sub menu
      $this.find('.mega_dropmenu').removeClass('show');
      $this.find('.drop_menu_1').addClass('show');
  
      $this.find('.dropmenu').removeClass('active');
      $this.find('[data-target=".drop_menu_1"]').addClass('active');
    });
  
    // Hide on leave
    jQuery(menuItemSelector).mouseleave(function () {
      jQuery(this).removeClass('active').children(dropdownSelector).removeClass('show');
    });
  
    // Hover on left items to toggle right and active class
    jQuery(subMenuLinkSelector).mouseenter(function () {
      const targetSelector = jQuery(this).data('target');
      const $dropdown = jQuery(this).closest('.mega_dropdown');
  
      $dropdown.find('.dropmenu').removeClass('active');
      jQuery(this).addClass('active');
  
      $dropdown.find('.mega_dropmenu').removeClass('show');
      $dropdown.find(targetSelector).addClass('show');
    });
  });
  
  jQuery(document).ready(function () {
    const menuItemSelector = '.menu-item.menudrop-item';
  
    jQuery(menuItemSelector).hover(
      function () {
        // Mouse enters
        jQuery('body').addClass('hove-text');
      },
      function () {
        // Mouse leaves
        jQuery('body').removeClass('hove-text');
      }
    );
  });


  
  $(document).ready(function () {
    // Hamburger toggle
    $(".c-hamburger").on("click", function (e) {
      e.preventDefault();
      $(this).toggleClass("is-active");
      $(".open").toggleClass("oppenned");
      $("body").toggleClass("active-overlay");
    });
  
    // Accordion for top-level <h3><a>
    $("#accordian > ul > li > h3 > a").on("click", function (e) {
      var parentLi = $(this).closest("li");
      var hasSubmenu = parentLi.children("ul").length > 0;
  
      if (hasSubmenu) {
        e.preventDefault();
  
        if (parentLi.hasClass("active")) {
          parentLi.removeClass("active").children("ul").slideUp(300);
        } else {
          // Close only top-level items, not nested ones
          $("#accordian > ul > li.active")
            .removeClass("active")
            .children("ul")
            .slideUp(300);
  
          parentLi.addClass("active").children("ul").slideDown(300);
        }
      }
    });
  
    // Toggle nested submenu on clicking .arrow links
    $("#accordian .arrow").on("click", function (e) {
      e.preventDefault();
      var $li = $(this).closest("li");
  
      if ($li.hasClass("active")) {
        $li.removeClass("active").children("ul").slideUp(300);
      } else {
        $li.addClass("active").children("ul").slideDown(300);
      }
    });
  
    // When clicking on the final submenu links, close menu (optional)
    $(".sub-menu li ul li a:not(.arrow)").on("click", function () {
      $(".open").removeClass("oppenned");
      $(".c-hamburger").removeClass("is-active");
      $("body").removeClass("active-overlay");
    });
  });
  

  $(function() {
  $('.mobile-bottom-menu li> a,.mobile-menu-mob .close_icon img, .servise_inner .close_icon img, .employer_wrap .close_icon img').click(function() {
    // If the clicked link already has the 'active' class, remove it
    if ($(this).hasClass('active')) {
      $(this).removeClass('active');
    } else {
      // If the clicked link does not have 'active', add it
      $(this).addClass('active');
    }

    // Remove the 'active' class from all other links
    $('.mobile-bottom-menu li a').not(this).removeClass('active');
  });

  $(".services, .servise_inner .close_icon img").on("click", function(){
  $(".services_wrap").toggleClass("show");
  $(".employer_wrap").removeClass("show1");
  $(".contact_wrap").removeClass("show2");
  $(".menu_wrap").removeClass("show3");
  });

  $(".employer, .employer_wrap .close_icon img").on("click", function(){
  $(".employer_wrap").toggleClass("show1");
  $(".services_wrap").removeClass("show");
  $(".contact_wrap").removeClass("show2");
  $(".menu_wrap").removeClass("show3");
  });

  $(".contact, .contact .close_icon img").on("click", function(){
  $(".contact_wrap").toggleClass("show2");
  $(".services_wrap").removeClass("show");
  $(".employer_wrap").removeClass("show1");
  $(".menu_wrap").removeClass("show3");
  });

  $(".menu, .mobile-menu-mob .close_icon img").on("click", function(){
  $(".menu_wrap").toggleClass("show3");
  $(".services_wrap").removeClass("show");
  $(".employer_wrap").removeClass("show1");
  $(".contact_wrap").removeClass("show2");
 // $(".mobile-bottom-menu > ul > li > a").removeClass("active");
  });
   });

      function toggleAccordion(header) {
    const content = header.nextElementSibling;
    const icon = header.querySelector('.toggle-icon');
    const isOpen = content.classList.contains('show');

    // Close all
    document.querySelectorAll('.accordion-content').forEach(el => el.classList.remove('show'));
    document.querySelectorAll('.accordion-title').forEach(el => {
      el.classList.remove('open');
      el.querySelector('.toggle-icon').textContent = '+';
    });

    // Toggle clicked
    if (!isOpen) {
      content.classList.add('show');
      header.classList.add('open');
      icon.textContent = '–';
    }
  }

  
function toggleMenu(header) {
  const menucontent = header.nextElementSibling;
  const menuicon = header.querySelector('.toggle-icon');
  const menuisOpen = menucontent.classList.contains('show');

  // Close all menu contents and reset icons
  document.querySelectorAll('.menu-content').forEach(el => el.classList.remove('show'));
  document.querySelectorAll('.menu-title').forEach(el => {
    el.classList.remove('open');
    const icon = el.querySelector('.toggle-icon');
    if (icon) icon.textContent = '+';
  });

  // Open the clicked one if not already open
  if (!menuisOpen) {
    menucontent.classList.add('show');
    header.classList.add('open');
    if (menuicon) menuicon.textContent = '–';
  }
}

document.addEventListener("DOMContentLoaded", function () {
  // ✅ Ensure correct toggle icon and class state on page load
  document.querySelectorAll('.menu-title').forEach(header => {
    const icon = header.querySelector('.toggle-icon');
    const content = header.nextElementSibling;
    const isOpen = header.classList.contains('open') && content.classList.contains('show');

    if (icon) {
      icon.textContent = isOpen ? '–' : '+';
    }

    // Remove .open and .show if not About Us (first one)
    if (!isOpen) {
      header.classList.remove('open');
      content.classList.remove('show');
    }
  });

  // ✅ Submenu (nested UL) toggle
  document.querySelectorAll('.menu-content li > a').forEach(link => {
    const submenu = link.nextElementSibling;

    if (submenu && submenu.tagName === 'UL') {
      link.parentElement.classList.add('has-submenu');

      link.addEventListener('click', function (e) {
        e.preventDefault();
        submenu.classList.toggle('submenu-open');
         
        link.classList.toggle('submenu-opened');
        
      });
    }
  });
  
  
});

$(document).on("click", ".has-submenu ul li a", function () {
  $(".menu_wrap").removeClass("show3");
});
