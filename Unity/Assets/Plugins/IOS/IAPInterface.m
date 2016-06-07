//
//  UJSInterface.m
//  Unity-iPhone
//
//  Created by MacMini on 14-5-15.
//
//

#import "IAPInterface.h"
#import "MLIAPManager.h"
//static IAPInterface * sharedInstance = nil;
@interface IAPInterface() <MLIAPManagerDelegate>

@end

@implementation IAPInterface



void TestMsg(){
    NSLog(@"Msg received");

}

void TestSendString(void *p){
    NSString *list = [NSString stringWithUTF8String:p];
    NSArray *listItems = [list componentsSeparatedByString:@"\t"];
    
    for (int i =0; i<listItems.count; i++) {
        NSLog(@"msg %d : %@",i,listItems[i]);
    }
    
}

void TestGetString(){
    NSArray *test = [NSArray arrayWithObjects:@"t1",@"t2",@"t3", nil];
    NSString *join = [test componentsJoinedByString:@"\n"];
    UnitySendMessage("UI Root", "IOSToU", [join UTF8String]);
}


void RequstProductInfo(void *p){
    NSString *list = [NSString stringWithUTF8String:p];
    NSLog(@"productKey:%@",list);
}


void InitIAPManager(){
    IAPInterface * ipa = [[IAPInterface alloc] init];
    [MLIAPManager sharedManager].delegate = ipa;
}

bool IsProductAvailable(){
    return [SKPaymentQueue canMakePayments];
}


void BuyProduct(void *p){
    NSString *str  = [NSString stringWithUTF8String: p];
    [[MLIAPManager sharedManager] requestProductWithId:str];
}


@end
