<template>
    <div class="donation-form bgcolor-theme3">
        <div class="section-title">
            <h5 class="subtitle line-theme-color mb-14">Make A Donation</h5>
            <h2 class="title text-white">Donate Now</h2>
            <img class="line-shape" src="/images/shape/line-t3.png" alt="image">
        </div>
        <form>
            <div class="amount-info">
                <div class="donate-amount">$20</div>
                <div class="donate-amount">$50</div>
                <div class="donate-amount">$200</div>
                <div class="donate-amount donate-custom-amount">
                    <input class="form-control" type="text" placeholder="Custom">
                </div>
            </div>
            <div class="payment-method-wrp">
                <h4>Payment Method:</h4>
                <div class="payment-method">
                    <div class="payment-type">
                        <div class="form-check form-check-inline">
                            <input class="form-check-input" type="radio" name="inlineRadioOptions" id="inlineRadio1" value="option1">
                            <label class="form-check-label" for="inlineRadio1">Offline</label>
                        </div>
                        <div class="form-check form-check-inline">
                            <input class="form-check-input" type="radio" name="inlineRadioOptions" id="inlineRadio2" value="option2">
                            <label class="form-check-label" for="inlineRadio2">Credit Card</label>
                        </div>
                    </div>
                    <img src="/images/photos/payment-card.png" alt="image">
                </div>
            </div>
            <div class="personal-info">
                <h4>Personal Info:</h4>
                <div class="row row-gutter-20">
                    <div class="col-md-6">
                        <div class="form-group">
                            <input class="form-control" type="text" name="con_name" placeholder="Name">
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <input class="form-control" type="email" name="con_email" placeholder="Email">
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <input class="form-control" type="text" name="con_phone" placeholder="Phone">
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <input class="form-control" type="text" name="con_address" placeholder="Address">
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="form-group mb-0">
                            <textarea class="form-control textarea" name="con_message" placeholder="Message"></textarea>
                        </div>
                    </div>
                </div>
            </div>
            <div class="btn-wrp">
                <a href="#" class="btn-theme btn-gradient btn-slide">Donate Now <img class="icon icon-img" src="/images/icons/arrow-line-right2.png" alt="Icon"></a>
                <a href="#" class="btn-theme btn-gradient btn-border">Total Donation: $50</a>
            </div>
        </form>
        <div class="layer-style">
            <img class="layer-style1" src="/images/shape/form-shape1.png" alt="image">
            <img class="layer-style2" src="/images/shape/form-shape2.png" alt="image">
        </div>
    </div>
</template>

<script>
    export default {

    };
</script>