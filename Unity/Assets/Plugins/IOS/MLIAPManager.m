//
//  MLIAPManager.m
//  MLIAPurchaseManager
//
//  Created by mali on 16/5/14.
//  Copyright © 2016年 mali. All rights reserved.
//

#import "MLIAPManager.h"

static MLIAPManager * sharedInstance = nil;

@interface MLIAPManager() <SKProductsRequestDelegate, SKPaymentTransactionObserver> {
    SKProduct *myProduct;
}


@end

@implementation MLIAPManager

#pragma mark - ****************  Singleton

+ (instancetype)sharedManager {
    
    @synchronized(self) {
        if (sharedInstance == nil) {
            sharedInstance = [MLIAPManager new];
            return sharedInstance;
        }
        else{
            return sharedInstance;
        }
    }
    return nil;
}

#pragma mark - ****************  Public Methods

/** TODO:请求商品*/
- (BOOL)requestProductWithId:(NSString *)productId {
    
    if (productId.length > 0) {
        NSLog(@"请求商品: %@", productId);
        SKProductsRequest *productRequest = [[SKProductsRequest alloc]initWithProductIdentifiers:[NSSet setWithObject:productId]];
        productRequest.delegate = self;
        [productRequest start];
        return YES;
    } else {
        NSLog(@"商品ID为空");
    }
    return NO;
}

/** TODO:购买商品*/
- (BOOL)purchaseProduct:(SKProduct *)skProduct {
    
    if (skProduct != nil) {
        if ([SKPaymentQueue canMakePayments]) {
            SKPayment *payment = [SKPayment paymentWithProduct:skProduct];
            [[SKPaymentQueue defaultQueue] addTransactionObserver:self];
            [[SKPaymentQueue defaultQueue] addPayment:payment];
            return YES;
        } else {
            NSLog(@"失败，用户禁止应用内付费购买.");
        }
    }
    return NO;
}

/** TODO:非消耗品恢复*/
- (BOOL)restorePurchase {
    
    if ([SKPaymentQueue canMakePayments]) {
        [[SKPaymentQueue defaultQueue] addTransactionObserver:self];
        [[SKPaymentQueue defaultQueue]restoreCompletedTransactions];
        return YES;
    } else {
        NSLog(@"失败,用户禁止应用内付费购买.");
    }
    return NO;
}


#pragma mark - ****************  SKProductsRequest Delegate

- (void)productsRequest:(SKProductsRequest *)request didReceiveResponse:(SKProductsResponse *)response {
    
    NSArray *myProductArray = response.products;
    if (myProductArray.count > 0) {
        myProduct = [myProductArray objectAtIndex:0];
        [self receiveProduct:myProduct];
    } else {
        NSLog(@"无法获取产品信息，购买失败。");
        [self receiveProduct:myProduct];
    }
}

#pragma mark - ****************  SKPaymentTransactionObserver Delegate

- (void)paymentQueue:(SKPaymentQueue *)queue updatedTransactions:(NSArray<SKPaymentTransaction *> *)transactions {
    
    for (SKPaymentTransaction *transaction in transactions) {
        switch (transaction.transactionState) {
            case SKPaymentTransactionStatePurchasing: //商品添加进列表
                NSLog(@"商品:%@被添加进购买列表",myProduct.localizedTitle);
                break;
            case SKPaymentTransactionStatePurchased://交易成功
                [self completeTransaction:transaction];
                break;
            case SKPaymentTransactionStateFailed://交易失败
                [self failedTransaction:transaction];
                break;
            case SKPaymentTransactionStateRestored://已购买过该商品
                break;
            case SKPaymentTransactionStateDeferred://交易延迟
                break;
            default:
                break;
        }
    }
}



#pragma mark - ****************  Private Methods

- (void)completeTransaction:(SKPaymentTransaction *)transaction {
    
    NSURL *receiptUrl = [[NSBundle mainBundle] appStoreReceiptURL];
    NSData *receiptData = [NSData dataWithContentsOfURL:receiptUrl];
    [self successfulPurchaseOfId:transaction.payment.productIdentifier andReceipt:receiptData];
    [[SKPaymentQueue defaultQueue] finishTransaction:transaction];
}


- (void)failedTransaction:(SKPaymentTransaction *)transaction {
    
    if (transaction.error.code != SKErrorPaymentCancelled && transaction.error.code != SKErrorUnknown) {
        [self failedPurchaseWithError:transaction.error.localizedDescription];
    }
    [[SKPaymentQueue defaultQueue] finishTransaction: transaction];
}


#pragma mark - **************** MLIAPManager Delegate

- (void)receiveProduct:(SKProduct *)product {
    
    if (product != nil) {
        //购买商品
        if (![[MLIAPManager sharedManager] purchaseProduct:product]) {
            UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"失败" message:@"您禁止了应用内购买权限,请到设置中开启" delegate:self cancelButtonTitle:@"关闭" otherButtonTitles:nil, nil];
            [alert show];
        }
    } else {
        UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"失败" message:@"无法连接App store!" delegate:self cancelButtonTitle:@"关闭" otherButtonTitles:nil, nil];
        [alert show];
    }
}

- (void)successfulPurchaseOfId:(NSString *)productId andReceipt:(NSData *)transactionReceipt {
    NSLog(@"购买成功");
    
    NSString  *transactionReceiptString = [transactionReceipt base64EncodedStringWithOptions:0];
    
    if ([transactionReceiptString length] > 0) {
        // 向自己的服务器验证购买凭证（此处应该考虑将凭证本地保存,对服务器有失败重发机制）
        /**
         服务器要做的事情:
         接收ios端发过来的购买凭证。
         判断凭证是否已经存在或验证过，然后存储该凭证。
         将该凭证发送到苹果的服务器验证，并将验证结果返回给客户端。
         如果需要，修改用户相应的会员权限
         */
        UnitySendMessage("UI Root", "ProvideContent", [transactionReceiptString  UTF8String]);
    }
}

- (void)failedPurchaseWithError:(NSString *)errorDescripiton {
    NSLog(@"购买失败");
    UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"失败" message:errorDescripiton delegate:self cancelButtonTitle:@"关闭" otherButtonTitles:nil, nil];
    [alert show];
}

@end
