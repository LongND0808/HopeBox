<template>
    <div class="widget">
        <h3 class="widget-title">Urgent Causes</h3>
        <div class="separator-line">
            <img src="/images/shape/line-t2.png" alt="image">
        </div>
        <div class="widget-causes-item">
            <div class="thumb">
                <img src="/images/causes/w1.jpg" alt="image">
            </div>
            <div class="content">
                <h4 class="title">
                    <nuxt-link to="/causes-details">Need School For Uganda Poor Children.</nuxt-link>
                </h4>
                <ul class="donate-info">
                    <li class="info-item">
                        <span class="info-title">Goal:</span>
                        <span class="amount">$ 5,000</span>
                    </li>
                    <li class="info-item">
                        <span class="info-title">Raised:</span>
                        <span class="amount">$ 2,000</span>
                    </li>
                </ul>
                <!-- Start Progress Item -->
                <div class="progress-item">
                    <div class="progress-line">
                        <div class="progress-bar-line" style="width: 75%">
                            <span class="percent">75%</span>
                        </div>
                    </div>
                </div>
                <form>
                    <div class="amount-info">
                        <div class="donate-amount">$20</div>
                        <div class="donate-amount">$35</div>
                        <div class="donate-amount">$48</div>
                        <div class="donate-amount mr-0">$90</div>
                        <div class="donate-amount donate-custom-amount">
                            <input class="form-control" type="text" placeholder="Custom amount">
                        </div>
                    </div>
                    <a class="btn-theme btn-gradient btn-slide" href="javascript:void(0)">
                        <span>Donate Now <img class="icon icon-img" src="/images/icons/arrow-line-right2.png" alt="Icon"></span>
                    </a>
                </form>
            </div>
        </div>
    </div>
</template>

<script>
    export default {

    };
</script>